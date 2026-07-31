using Dsw2026Ej15.Data;
using Dsw2026Ej15.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Dsw2026Ej15.Data.Util
{
    public static class DataExtensions
    {
        public static void Seedwork<T>(this Dsw2026Ej16DbContext context, string fileName) where T : BaseEntity
        {
            if (!context.Set<T>().Any())
            {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources", $"{fileName}.json");
                string jsonContent = File.ReadAllText(jsonPath);

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var entities = JsonSerializer.Deserialize<List<T>>(jsonContent, options);

                if (entities != null && entities.Any())
                {
                    context.Set<T>().AddRange(entities);
                    context.SaveChanges();
                }
            }
        }
    }
}