﻿using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Service.Helpers;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly IOptions<JwtConfiguration> _configuraiton;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly EntityChecker _entityChecker;

        private User? _user;

        public AuthenticationService(IRepositoryManager repositoryManager, IMapper mapper, IOptions<JwtConfiguration> configuraiton, EntityChecker entityChecker)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _configuraiton = configuraiton;
            _jwtConfiguration = _configuraiton.Value;
            _entityChecker = entityChecker;
        }

        public async Task<UserDTO> CreateUserAsync(UserForRegistrationDTO userForRegistration)
        {
            var user = await _repositoryManager.User.GetUserByEmailAsync(userForRegistration.Nickname, trackChanges: false);
            if (user is not null)
                throw new BadRequestException($"User with email {user.Email} already exist");

            await _entityChecker.GetUserRankAndCheckIfItExist((Guid)userForRegistration.UserRankId, trackChanges: false);
            await _entityChecker.GetUserByNicknameAndCheck(userForRegistration.Nickname, trackChanges: false);

            userForRegistration.Password = PasswordHash.EncodePasswordToBase64(userForRegistration.Password);

            user = _mapper.Map<User>(userForRegistration);

            _repositoryManager.User.CreateUser(user);
            await _repositoryManager.SaveAsync();

            var userToReturn = _mapper.Map<UserDTO>(user);
            return userToReturn;
        }

        public async Task<bool> ValidateUser(UserForAuthenticationDTO userForAuthentication)
        {
            _user = await _repositoryManager.User.GetUserByNicknameAsync(userForAuthentication.Nickname, trackChanges: true);
            return _user is not null && _user.Password == userForAuthentication.Password;
        }

        public async Task<TokenDTO> CreateToken(bool populateExp)
        {
            var signingCredentials = GetSigningCreadetials();
            var claims = GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            var refreshToken = GenerateRefreshToken();

            _user.RefreshToken = refreshToken;
            if (populateExp)
                _user.RefreshTokenExpires = DateTime.Now.AddDays(Convert.ToDouble(_jwtConfiguration.RefreshTokenExpiresDays));

            await _repositoryManager.SaveAsync();
            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenDTO(accessToken, refreshToken);
        }

        private SigningCredentials GetSigningCreadetials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private List<Claim> GetClaims()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, _user.Nickname)
            };
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOpt = new JwtSecurityToken
            (
                issuer: _jwtConfiguration.ValidIssuer,
                audience: _jwtConfiguration.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.ValidExpires)),
                signingCredentials: signingCredentials
            );   
            return tokenOpt;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using(var rnd = RandomNumberGenerator.Create())
            {
                rnd.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetClaimsPrincipialFromExpairedToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey)),
                ValidAudience = _jwtConfiguration.ValidAudience,
                ValidIssuer = _jwtConfiguration.ValidIssuer
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principial = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid access token");
            return principial;
        }

        public async Task<TokenDTO> RefreshToken(TokenDTO tokenDTO)
        {
            var principial = GetClaimsPrincipialFromExpairedToken(tokenDTO.AccessToken);

            var user = await _repositoryManager.User.GetUserByNicknameAsync(principial.Identity.Name, trackChanges : true);
            if(user is null
                || user.RefreshToken != tokenDTO.RefreshToken
                || user.RefreshTokenExpires <= DateTime.Now)
            {
                throw new BadRequestException("Refresh token was expired or invalid");
            }
            _user = user;
            return await CreateToken(populateExp: false);
        }
    }
}