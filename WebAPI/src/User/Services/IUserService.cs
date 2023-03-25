using System;
using BusinessObject.Models;

namespace WebAPI.Services;

public interface IUserService
{
	public User GetUserById(int id);
}

