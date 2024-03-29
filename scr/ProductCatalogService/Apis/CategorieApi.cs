﻿namespace ProductCatalogService.Apis;

public class CategorieApi : IApi
{
    const string endpoint = "/categories";
    public void Register(WebApplication app)
    {
        app.MapPost($"{endpoint}/create", AddCategorie)
            .Accepts<ICategorie>("application/json")
            .Produces<ICategorie>(StatusCodes.Status201Created)
            .WithName("CreateCategorie")
            .WithTags("Create");


        app.MapGet(endpoint, GetCategorie)
            .Produces<List<IBrand>>(StatusCodes.Status200OK)
            .WithName("GetAllCategories")
            .WithTags("Read");


        app.MapPut($"{endpoint}/update", Update)
            .Accepts<IProduct>("application/json")
            .WithName("UpdateCategorie")
            .WithTags("Update");


        app.MapDelete($"{endpoint}/delete", (int id, bool isAdmin, ICategorieRepository repository) => Delete(id, isAdmin, repository))
            .WithMetadata(new HttpMethodMetadata(new[] { "DELETE" }))
            .WithName("DeleteCategorie")
            .WithTags("Delete");


    }

    private async Task<IResult> AddCategorie([FromBody] Categorie data, bool isAdmin, ICategorieRepository repository)
    {
        if (!isAdmin) return Results.StatusCode(StatusCodes.Status403Forbidden);

        await repository.AddCategorieAsync(data, isAdmin);
        //return Results.Ok($"Добавили новый бренд с ID {brend.BrandId}");
        return Results.Created($"{endpoint}/{data.Id}", data);
    }



    private async Task<IResult> GetCategorie(ICategorieRepository repository) => await repository.GetCategoriesAsync() is List<Categorie> data
        ? Results.Ok(data)
        : Results.NotFound();

    private async Task<IResult> Delete(int id, bool isAdmin, ICategorieRepository repository)
    {
        if (!isAdmin) return Results.StatusCode(StatusCodes.Status403Forbidden);

        bool checkDelete = await repository.DeleteCategorieAsync(id, isAdmin);
        if (checkDelete) return Results.Ok($"Бренд с ID {id} успешно удален");
        else return Results.NotFound();
    }

    private async Task<IResult> Update(int idCategorie, bool isAdmin,[FromBody] Categorie data, ICategorieRepository repository)
    {
        if (!isAdmin) return Results.StatusCode(StatusCodes.Status403Forbidden);

        bool check = await repository.UpdateAsync(idCategorie, data, isAdmin);
        if (check) return Results.Ok($"Категория с ID {idCategorie} успешно обновлен");
        else return Results.NotFound();

    }






}