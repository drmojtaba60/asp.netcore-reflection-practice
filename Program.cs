using asp.netcore_reflection_practice.Enums;
using asp.netcore_reflection_practice.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/health-api", async () => $"Hi,{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}");
app.MapGet("/enums/myenum1", () => new
{
    names = Enum.GetNames(typeof(MyEnum01)),
    values = Enum.GetValues(typeof(MyEnum01)),
    
}
);
app.MapGet("/enums/myenum2", () =>
{

    IEnumerable<KeyValueItem> getEnumAsList(Type enumType)
    //IEnumerable<string> getEnumAsList(Type enumType)
    {
        //var dicResult=new Dictionary<int ,string>();
        //Type enumType = typeof(TEnum);
        if (!enumType.IsEnum)
        {
            throw new ArgumentException("TEnum must be an enum type.");
        }

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return Enum.GetValues(enumType)
            .Cast<Enum>()
            .Select(value =>
            {
                var fieldInfo = value.GetType().GetField(value.ToString());
                var displayAttribute = fieldInfo?.GetCustomAttributes(typeof(DisplayAttribute), false)
                    .OfType<DisplayAttribute>()
                    .FirstOrDefault();
                return new KeyValueItem(Convert.ToInt32(value), displayAttribute != null ? displayAttribute.Name : value.ToString());
                //dicResult.Add(int.Parse(value?.ToString() ?? "0"), displayAttribute?.Name ?? "");
                //return displayAttribute != null ? displayAttribute.Name : value.ToString();
            }).ToList();


#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }

    return getEnumAsList(typeof(MyEnum02));

});
app.MapGet("/enums/myenum2-by-names", () =>
{

    IEnumerable<KeyValueItem> getEnumAsList(Type enumType)
    //IEnumerable<string> getEnumAsList(Type enumType)
    {
        //var dicResult=new Dictionary<int ,string>();
        //Type enumType = typeof(TEnum);
        if (!enumType.IsEnum)
        {
            throw new ArgumentException("TEnum must be an enum type.");
        }

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return Enum.GetValues(enumType)
            .Cast<Enum>()
            .Select(value =>
            {
                //var fieldInfo = value.GetType().GetField(value.ToString());
                //var displayAttribute = fieldInfo?.GetCustomAttributes(typeof(DisplayAttribute), false)
                //    .OfType<DisplayAttribute>()
                //    .FirstOrDefault();
                return new KeyValueItem(Convert.ToInt16(value),$"{value}:{value.GetDisplayName()}");
                //dicResult.Add(int.Parse(value?.ToString() ?? "0"), displayAttribute?.Name ?? "");
                //return displayAttribute != null ? displayAttribute.Name : value.ToString();
            }).ToList();


#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }

    return getEnumAsList(typeof(MyEnum02));

});
app.MapGet("/enums/myenum2-labels", () =>
{

    //IEnumerable<string> getEnumAsList(Type enumType)
    ////IEnumerable<string> getEnumAsList(Type enumType)
    //{
    //Assembly assembly = typeof(MyEnum02).Assembly;  
    //Assembly assembly = Assembly.GetExecutingAssembly();
    //var enums = AppDomain.CurrentDomain.GetAssemblies()
    //      .Where(x => x.FullName.Contains("Csis.CulturalCamp.Domain")).FirstOrDefault()
    //      .GetTypes().Where(t => t.GetCustomAttributes(typeof(CsisEnumConditionAttribute), false).Any());

    Assembly assembly = Assembly.GetExecutingAssembly();
    var enums = assembly.GetTypes().Where(t => t.IsEnum);

    var enumsData = enums.Select(e =>
    {
        var i = new
        {
            Item = new
            {
                e.Name,
                Items = new List<KeyValueItem>() { }
            }

        };
        var enumValues = Enum.GetValues(e);
        foreach (var enumValue in enumValues)
        {
            i.Item.Items.Add(new KeyValueItem
            (
                Id: (int)enumValue,
                Value: ((Enum)enumValue).GetDisplayName()
            )
            );
        }
        return i;
    });
    return enumsData;


}
);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
