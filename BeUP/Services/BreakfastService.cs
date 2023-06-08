using BeUP.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;
using System.Reflection;
using System.Collections.ObjectModel;

namespace BeUP.Services;

public static class BreakfastService
{
    static SQLiteAsyncConnection db;
    private static string databasePath = Path.Combine(FileSystem.AppDataDirectory, "Resources.DataBases.DataBase.db");

    public static async Task Init()
    {
        if (db != null)
            return;

        db = new SQLiteAsyncConnection(databasePath);

        await db.CreateTableAsync<Breakfast>();

    }

    public static async Task<IEnumerable<Breakfast>> GetBreakfasts()
    {
        await Init();

        var breakfast = await db.Table<Breakfast>().ToListAsync();

        if (breakfast.Count == 0) //{ }
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("BeUP.Resources.DataBases.DataBase.db"))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);

                    File.WriteAllBytes(databasePath, memoryStream.ToArray());
                }
            }

            breakfast = await db.Table<Breakfast>().ToListAsync();
        }

        return breakfast;
    }

    public static async Task UpdateAllData(Collection<Breakfast> breakfasts)
    {
        await Init();

        await db.UpdateAllAsync(breakfasts);
    }

    public static async Task AddRecord(Breakfast breakfast)
    {
        await Init();

        await db.InsertAsync(breakfast);
    }

    public static async Task SaveChanges(Breakfast breakfast)
    {
        await Init();

        await db.UpdateAsync(breakfast);
    }

    public static async Task DeleteBreakfast(Breakfast breakfast)
    {
        await Init();

        await db.DeleteAsync(breakfast);
    }
}
