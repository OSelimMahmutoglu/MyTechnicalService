using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MTS.BLL.Settings;
using MTS.Models.IdentityModels;
using MTS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static MTS.BLL.Account.MemberShipTools;
using System.Web.Mvc;


namespace MTS.UI.MVC.Areas.Yonetim.Controllers
{
    
    public class UserController : Controller
    {
        // GET: Yonetim/User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Kayit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Kayit(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userManager = NewUserManager();
            var checkUser = userManager.FindByEmail(model.Email);
            if (checkUser != null)
            {
                ModelState.AddModelError("", "Bu email adresi kullanılmaktadır");
                return View(model);
            }
            checkUser = await userManager.FindByNameAsync(model.Username);
            if (checkUser != null)
            {
                ModelState.AddModelError("", "Bu kullanici adı kullanılmaktadır");
                return View(model);
            }
            var actcode = Guid.NewGuid().ToString().Replace("-", "");
            var user = new ApplicationUser()
            {
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.Phone,
                Surname = model.Surname,
                UserName = model.Username,
                ActivationCode = actcode
            };
            bool adminMi = userManager.Users.Count() == 0;
            var sonuc = await userManager.CreateAsync(user, model.ConfirmPassword);
            if (sonuc.Succeeded)
            {
                if (adminMi)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
                else
                {
                    if (model.OperatorMu)
                        userManager.AddToRole(user.Id, "Operator");
                    else
                        userManager.AddToRole(user.Id, "Teknisyen");
                }
                await SiteSettings.SendMail(new MailModel()
                {
                    To = user.Email,
                    Subject = "Hoşgeldiniz",
                    Message = $"Merhaba {user.UserName}, <br/>Sisteme başarıyla kaydoldunuz<br/>Hesabınızı aktifleştirmek için <a href='{SiteUrl()}/hesap/aktivasyon?code={actcode}'>Aktivasyon Kodu</a>"
                });

                return RedirectToAction("Index", "Main");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı kayıt işleminde hata oluştu!");
                return View(model);
            }
        }
        public ActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Giris(LoginViewModel model)
        {
            
            if (!ModelState.IsValid)
                return View(model);

            var userManager = NewUserManager();
            var user = await userManager.FindAsync(model.NameEmail, model.Password);
            if (user == null)
            {
                var emailuser = await userManager.FindByEmailAsync(model.NameEmail);
                if (emailuser == null)
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı");
                    return View(model);
                }
                user = await userManager.FindAsync(emailuser.UserName, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı");
                    return View(model);
                }
                else
                {
                    
                    if (!userManager.IsInRole(user.Id, "Admin"))
                    {
                        ModelState.AddModelError(string.Empty, "Only Admins can reach this panel");
                        return View(model);
                        
                    }
                    else { login(user);}
                    
                    //emaile göre giriş yaptı
                    
                }
            }
            else
            {
                // kullanıcı adına göre giriş yaptı
                if (!userManager.IsInRole(user.Id, "Admin"))
                {
                    ModelState.AddModelError(string.Empty, "Only Admins can reach this panel");
                    return View(model);
                }
                else { login(user); }
            }

            void login(ApplicationUser loginuser)
            {
                var authManager = HttpContext.GetOwinContext().Authentication;
                var userIdentity =
                    userManager.CreateIdentity(loginuser, DefaultAuthenticationTypes.ApplicationCookie);

                authManager.SignIn(new AuthenticationProperties()
                {
                    IsPersistent = model.RememberMe
                }, userIdentity);
            }
            return RedirectToAction("Index", "Main");
        }
        public ActionResult SifremiUnuttum()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> SifremiUnuttum(string name)
        {
            var userStore = NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userManager.FindByName(name);
            var user2 = userManager.FindByEmail(name);
            if (user == null && user2 == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı Bulunamadı");
                return View();
            }
            var kullanici = user != null ? user : user2;
            string parola = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            await userStore.SetPasswordHashAsync(kullanici, userManager.PasswordHasher.HashPassword(parola));
            await userStore.UpdateAsync(kullanici);
            await userStore.Context.SaveChangesAsync();
            await SiteSettings.SendMail(new MailModel
            {
                To = kullanici.Email,
                Subject = "Yeni parolanız",
                Message = $"Merhaba {kullanici.UserName},<br/>Yeni parolanız: <b>{parola}</b><br/><a href='{SiteUrl()}/hesap/giris'>Giriş Yap</a>"
            });
            return RedirectToAction("Index", "Main");
        }
        public ActionResult Cikis()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Main");
        }
        [Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Profilim()
        {
            string id = HttpContext.User.Identity.GetUserId();
            var userManager = NewUserManager();
            var user = await userManager.FindByIdAsync(id);
            var model = new ProfileViewModel()
            {
                Username = user.UserName,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                Phone = user.PhoneNumber
            };
            return View(model);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Profilim(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var userStore = NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);

            var user = await userManager.FindByIdAsync(HttpContext.User.Identity.GetUserId());

            user.Email = model.Email;
            user.PhoneNumber = model.Phone;

            await userStore.UpdateAsync(user);
            await userStore.Context.SaveChangesAsync();

            return RedirectToAction("Profilim");
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> SifreGuncelle(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Profilim", model);
            var userStore = NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userManager.FindById(HttpContext.User.Identity.GetUserId());

            var checkUser = userManager.Find(user.UserName, model.OldPassword);
            if (checkUser == null)
            {
                ModelState.AddModelError(string.Empty, "Mevcut şifreniz yanlış");
                return View("Profilim", model);
            }
            await userStore.SetPasswordHashAsync(user, userManager.PasswordHasher.HashPassword(model.ConfirmPassword));
            await userStore.UpdateAsync(user);
            await userStore.Context.SaveChangesAsync();

            return RedirectToAction("Cikis");
        }

        public string SiteUrl()
        {
            return Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host +
(Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
        }
    }
}