using Microsoft.AspNetCore.Mvc;
using ShortenURL.Application;

namespace ShortenURL.Api.Controllers;
//Контроллер для редиректа по короткой ссылке. Основная цель — перенаправлять пользователя и учитывать количество кликов

[Route("r")]
public class RedirectController : Controller
{
    private readonly IShortLinkRepository _repository;

    public RedirectController(IShortLinkRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{code}")]                                                     //Редирект по коду короткой ссылки
    public async Task<IActionResult> Go(string code)
    {
        var link = await _repository.GetByCodeAsync(code);                  //Ищем запись по уникальному коду, чтобы знать куда редиректить.

        if (link == null)
            return NotFound();

        await _repository.IncrementClicksAsync(code);                       // Мы не подгружаем всю сущность через EF (GetByCodeAsync), чтобы просто увеличить ClickCount

        return Redirect(link.OriginalUrl);
    }
}
