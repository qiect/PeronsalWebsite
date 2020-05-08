﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PersonalWebsite.DTO;
using PersonalWebsite.IService;
using PersonalWebsite.Service;
using PersonalWebsite.Service.Entity;

namespace ZSZ.Service
{
    public class FilterWordService : IFilterWordService
    {
        //重用
        private static string bannedExprKey = typeof(FilterWordService) + "BannedExpr";
        private static string modExprKey = typeof(FilterWordService) + "ModExpr";


        public long Add(string wordPattern, string replaceWord)
        {
            using (MyDbContext ctx = new MyDbContext())
            {
                FilterWordEntity filterWord = new FilterWordEntity();
                filterWord.WordPattern = wordPattern;
                filterWord.ReplaceWord = replaceWord;
                ctx.SaveChanges();
                return filterWord.Id;
            }
        }

        public FilterWordDTO[] GetAll()
        {
            using (MyDbContext ctx = new MyDbContext())
            {
                return ctx.FilterWords.Select(p => ToDTO(p)).ToArray();
            }
        }
        public FilterWordDTO[] GetAll(int page, int limit)
        {
            using (MyDbContext ctx = new MyDbContext())
            {
                var list = ctx.FilterWords.OrderByDescending(p => p.Id).Skip(limit).Take(page);
                return list.Select(p => ToDTO(p)).ToArray();
            }
        }

        public FilterWordDTO GetById(long id)
        {
            using (MyDbContext ctx = new MyDbContext())
            {
                return ToDTO(ctx.FilterWords.SingleOrDefault(p => p.Id == id));
            }
        }

        private FilterWordDTO ToDTO(FilterWordEntity entity)
        {
            FilterWordDTO dto = new FilterWordDTO();
            dto.Id = entity.Id;
            dto.CreateDateTime = entity.CreateDateTime;
            dto.WordPattern = entity.WordPattern;
            dto.ReplaceWord = entity.ReplaceWord;
            dto.IsDeleted = entity.IsDeleted ? "是" : "否";
            dto.DeletedDateTime = entity.DeletedDateTime;
            return dto;
        }

        public FilterWordDTO[] GetBanned()
        {
            using (MyDbContext ctx = new MyDbContext())
            {
                return ctx.FilterWords.Where(p => p.ReplaceWord == "{BANNED}").Select(p => ToDTO(p)).ToArray();
            }
        }
        public FilterWordDTO[] GetMod()
        {
            using (MyDbContext ctx = new MyDbContext())
            {
                return ctx.FilterWords.Where(p => p.ReplaceWord == "{MOD}").Select(p => ToDTO(p)).ToArray();
            }
        }
        public FilterWordDTO[] GetReplace()
        {
            using (MyDbContext ctx = new MyDbContext())
            {
                return ctx.FilterWords.Where(p => p.ReplaceWord != "{MOD}" && p.ReplaceWord != "{BANNED}").Select(p => ToDTO(p)).ToArray();
            }
        }




    }

    public enum FilterResult
    {
        OK, Mod, Banned
    }
}
