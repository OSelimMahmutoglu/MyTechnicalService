﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using MTS.BLL.Settings;
using MTS.Models.IdentityModels;
using MTS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static MTS.BLL.Account.MemberShipTools;

namespace MTS.UI.MVC.Controllers
{
    public class HesapController : Controller
    {
        // GET: Hesap
        public ActionResult Kayit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                userManager.AddToRole(user.Id, "Musteri");

                await SiteSettings.SendMail(new MailModel()
                {
                    To = user.Email,
                    Subject = "Hoşgeldiniz",
                    Message = $"Merhaba {user.UserName}, <br/>Sisteme başarıyla kaydoldunuz<br/>Hesabınızı aktifleştirmek için <a href='{SiteUrl()}/hesap/aktivasyon?code={actcode}'>Aktivasyon Kodu</a>"
                });

                return RedirectToAction("Giris", "Hesap");
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
                    //emaile göre giriş yaptı
                    var authManager = HttpContext.GetOwinContext().Authentication;
                    var userIdentity =
                        userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                    authManager.SignIn(new AuthenticationProperties()
                    {
                        IsPersistent = model.RememberMe
                    }, userIdentity);
                }
            }
            else
            {
                // kullanıcı adına göre giriş yaptı
                var authManager = HttpContext.GetOwinContext().Authentication;
                var userIdentity =
                    userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                authManager.SignIn(new AuthenticationProperties()
                {
                    IsPersistent = model.RememberMe
                }, userIdentity);
            }
            return RedirectToAction("Index", "Main");
        }

        [Authorize]
        public ActionResult Cikis()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Main");
        }
        [Authorize]
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
        public async Task<ActionResult> Aktivasyon(string code)
        {
            var userStore = NewUserStore();
            var sonuc = userStore.Context.Set<ApplicationUser>().Where(x => x.ActivationCode == code).FirstOrDefault();
            if (sonuc == null)
            {
                ViewBag.sonuc = "Aktivasyon başarısız";
                return View();
            }
            sonuc.EmailConfirmed = true;
            await userStore.UpdateAsync(sonuc);
            await userStore.Context.SaveChangesAsync();
            ViewBag.sonuc = "Aktivasyon başarılı";
            await SiteSettings.SendMail(new MailModel()
            {
                To = sonuc.Email,
                Subject = "Aktivasyon başarılı",
                Message = $"Merhaba {sonuc.UserName}, Aktivasyon işleminiz başarılı :)"
            });
            HttpContext.GetOwinContext().Authentication.SignOut();
            return View();
        }
        [Authorize]
        public async Task<ActionResult> TekrarAktivasyonGonder()
        {
            var userStore = NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userManager.FindById(HttpContext.User.Identity.GetUserId());
            await SiteSettings.SendMail(new MailModel()
            {
                To = user.Email,
                Subject = "Aktivasyon Kodu",
                Message = $"Merhaba {user.UserName}, <br/>Hesabınızı aktifleştirmek için <a href='{SiteUrl()}/hesap/aktivasyon?code={user.ActivationCode}'>Aktivasyon Kodu</a>"
            });

            return RedirectToAction("Profilim","Hesap");
        }
        public ActionResult SifremiUnuttum()
        {
            return View();
        }
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
        public string SiteUrl()
        {
            return Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host +
(Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
        }
    }
}
