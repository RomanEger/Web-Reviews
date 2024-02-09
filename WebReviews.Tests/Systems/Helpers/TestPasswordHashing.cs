using FluentAssertions;
using Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebReviews.Tests.Systems.Helpers
{
    public class TestPasswordHashing
    {
        [Fact]
        public void Get_OnSucces_Encoded_Password_1()
        {
            var password = "hjh34.2";
            var expectedPassword = "aGpoMzQuMg==";

            var encodedPasword = PasswordHash.EncodePasswordToBase64(password);

            encodedPasword.Should().BeEquivalentTo(expectedPassword);
        }

        [Fact]
        public void Get_OnSucces_Encoded_Password_2()
        {
            var password = "ffGGFf34";
            var expectedPassword = "ZmZHR0ZmMzQ=";

            var encodedPasword = PasswordHash.EncodePasswordToBase64(password);

            encodedPasword.Should().BeEquivalentTo(expectedPassword);
        }

        [Fact]
        public void Get_OnSucces_Decoded_Password_1()
        {
            var encodedPassword = "WG5qZmQzNDQ=";
            var expectedPassword = "Xnjfd344";

            var decodedPassword = PasswordHash.DecodeFrom64(encodedPassword);

            decodedPassword.Should().BeEquivalentTo(expectedPassword);
        }

        [Fact]
        public void Get_OnSucces_Decoded_Password_2()
        {
            var encodedPassword = "cXdlcnR5MTIz";
            var expectedPassword = "qwerty123";

            var decodedPassword = PasswordHash.DecodeFrom64(encodedPassword);

            decodedPassword.Should().BeEquivalentTo(expectedPassword);
        }
    }
}
