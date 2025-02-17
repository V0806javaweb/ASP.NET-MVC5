﻿using MemberSystem.Models;
using MemberSystem.Services;
using MemberSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemberSystem.Controllers
{
    public class GuestbookController : Controller
    {
        // table Guestbooks service object
        private readonly GuestbookDBService GuestbookService = new GuestbookDBService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetDataList(string Search,int Page = 1)
        {
            GuestbookViewModel Data = new GuestbookViewModel();
            Data.Search = Search;
            Data.Paging = new ForPaging(Page);
            Data.DataList = GuestbookService.GetDataList(Data.Paging, Data.Search);
            return PartialView(Data);
        }

        [HttpPost]
        public ActionResult GetDataList([Bind(Include = "Search")]GuestbookViewModel Data)
        {
            return RedirectToAction("GetDataList", new { Search = Data.Search });
        }

        #region 新增留言
        //add comment's initial page
        public ActionResult Create()
        {
            return PartialView();
        }

        //action about add or sent comment
        //HttpPost only
        //以Bind Include限制接受的欄位值
        [Authorize]
        [HttpPost]
        public ActionResult Create([Bind(Include ="Content")] Guestbook Data)
        {
            //紀錄留言人為當前登入者
            Data.Account = User.Identity.Name;
            GuestbookService.InsertGuestbook(Data);
            //back to Index page
            return RedirectToAction("Index");
        }
        #endregion

        #region 修改留言
        [Authorize]
        public ActionResult Edit(int Id)
        {
            Guestbook Data = GuestbookService.GetDataById(Id);
            return View(Data);
        }
        //action for 修改留言傳入資料
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int Id,[Bind(Include ="Content")]Guestbook UpdateData)
        {
            //use check method prevent update error
            if (GuestbookService.CheckUpdate(Id))
            {
                UpdateData.Id = Id;
                UpdateData.Account = User.Identity.Name;
                GuestbookService.UpdateGuestbook(UpdateData);
                //return RedirectToAction("Index");
            }
            /*else
            {
                return RedirectToAction("Index");
            }*/
            return RedirectToAction("Index");
        }
        #endregion

        #region 回覆留言
        [Authorize(Roles = "Admin")]
        public ActionResult Reply(int Id)
        {
            Guestbook Data = GuestbookService.GetDataById(Id);
            return View(Data);
        }

        //modify action
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Reply(int Id,[Bind(Include ="Reply,ReplyTime")]Guestbook ReplyData)
        {
            if (GuestbookService.CheckUpdate(Id))
            {
                ReplyData.Id = Id;
                GuestbookService.ReplyGuestbook(ReplyData);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region 刪除留言
        [Authorize(Roles = "Admin")] //管理員才有權刪留言
        public ActionResult Delete(int Id)
        {
            GuestbookService.DeleteGuestbook(Id);
            return RedirectToAction("Index");
        }
        #endregion

    }
}