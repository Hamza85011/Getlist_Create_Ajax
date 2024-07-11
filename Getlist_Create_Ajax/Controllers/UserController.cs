using CRUD_ADO.NET.Services;
using hamzacrud.Models;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using CRUD_ADO.NET.Models;
using System.Reflection;

public class UserController : Controller
{
    private readonly UserDAL _userDAL;

    public UserController(IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("cs");
        _userDAL = new UserDAL(connectionString);
    }

    public IActionResult GetList()
    {
        List<UserModel> users = _userDAL.GetList();
        return View(users);
    }
    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    public ActionResult Login(UserLogin model)
    {
        if (ModelState.IsValid)
        {
            // Call the AuthenticateUser method from the UserDAL
            bool isAuthenticated = _userDAL.AuthenticateUser(model.Username, model.Password);

            if (isAuthenticated)
            {
                // Successful login, redirect to the Getlist
                return RedirectToAction("User/GetList");
            }
            else
            {
                // Login failed, display an error message
                ModelState.AddModelError("", "Invalid username or password.");
            }
        }

        // If the model is not valid or login fails, return to the Login view with the error message
        return View(model);
    }

    public ActionResult CreateAccount()
    {
        return View();
    }
    [HttpPost]
    public ActionResult CreateAccount(UserLogin login)
    {
        if (ModelState.IsValid)
        {
            bool checkk;
            checkk = _userDAL.CreateAccount(login);
            if (checkk)
            {
                TempData["SuccessMessage"] = "Student record submitted successfully.";
                return Json(new { success = true }); // Return JSON response indicating success

            }
            else
            {
                return Json(new { Success = false, Message = "Failed to add student data. Please try again later." });
            }

        }
        return View();
    }


    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(UserModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.First_Name))
        {
            return View(model);
        }

        if (_userDAL.Create(model))
        {
            TempData["Message"] = "Student data added successfully.";
            // Return a JSON response with a success status
            return Json(new { Success = true });
        }
        else
        {
            TempData["InsertMsg"] = "Failed to save user data.";
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        UserModel model = _userDAL.GetDetails(id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult Update(UserModel model)
    {
        if (ModelState.IsValid)
        {
            if (_userDAL.Update(model))
            {
                TempData["InsertMsge"] = "User update your data successfully.";
                return RedirectToAction("GetList");
            }
            else
            {
                TempData["InsertMsg"] = "Failed to save user data.";
            }
        }

        return View(model);
    }

    public IActionResult Details(int id)
    {
        UserModel model = _userDAL.GetDetails(id);
        return View(model);
    }
    public IActionResult Delete(int id)
    {
        bool isDeleted = _userDAL.Delete(id);

        if (isDeleted)
        {
            TempData["DelectMsg"] = "User Delete your data successfully.";
        }
        else
        {
            TempData["DeleteMsg"] = "Failed to delete user data.";
        }
        return RedirectToAction("GetList");
    }
}
