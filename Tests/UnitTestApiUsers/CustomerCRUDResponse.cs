using Entities.Models;
using WebReviews.API.Controllers;
using Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UnitTestApiUsers;

public class CustomerCRUDResponse
{
    [Fact]
    public async void CreateNewUserAndReturnCode201()
    {
        //arrange
        var controller = new UserController();
        User newUser = new User()
        {
            Nickname = "John",
            Email = "John@example.com",
            Password = "123",
            UserRankId = Guid.Parse("d1ebd7e3-7ae0-4e4d-8e4f-06b95b60fcc6")
        };

        //act
        var response = (ObjectResult) await controller.CreateUser(newUser);

        //assert 
        Assert.Equal(StatusCodes.Status201Created, response.StatusCode);
    }
    [Fact]
    public async void GetUserById_ReturnStatusCode200()
    {
        //arrange
        var controller = new UserController();
        Guid id = Guid.Parse("ae376e02-f523-4120-b276-473ab3ef9c27");
        //act
        var response = (OkResult) await controller.GetUser(id);

        //assert 
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }
    [Fact]
    public async void UpdateUser_ReturnStatusCode200()
    {
        //arrange
        var controller = new UserController();
        Guid id = Guid.Parse("ae376e02-f523-4120-b276-473ab3ef9c27");
        User user = new User()
        {
            Nickname = "Andrew",
            Email = "John@example.com",
            Password = "123123",
            UserRankId = Guid.Parse("d1ebd7e3-7ae0-4e4d-8e4f-06b95b60fcc6")
        };

        //act
        var response = (OkResult) await controller.UpdateUser(id,user);

        //assert 
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }
    [Fact]
    public async void DeleteUserById_ReturnNoContent()
    {
        //arrange
        var controller = new UserController();
        
        Guid id = Guid.Parse("ae376e02-f523-4120-b276-473ab3ef9c27");

        //act
        var response = (NoContentResult) await controller.DeleteUser(id);

        //assert 
        Assert.Equal(StatusCodes.Status204NoContent, response.StatusCode);
    }
}