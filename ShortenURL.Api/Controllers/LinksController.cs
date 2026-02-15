using Microsoft.AspNetCore.Mvc;
using ShortenURL.Application;


namespace ShortenURL.Api.Controllers;
// Контроллер для управления короткими ссылками. Сосредотачивает логику работы с HTTP-запросами и делегирует бизнес-логику сервисам и репозиториям

public class LinksController : Controller
{
    private readonly ShortLinkService _service;
    private readonly IShortLinkRepository _repository;

    public LinksController(
        ShortLinkService service,
        IShortLinkRepository repository)
    {
        _service = service;
        _repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var links = await _repository.GetAllAsync();                                    //Возвращает View с GetAllAsync, чтобы пользователь видел актуальный список ссылок на главной странице
        return View(links);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string originalUrl)                         // Try/Catch чтобы перехватывать ошибки валидации и не рушить страницу
    {                                                                                   // TempData чтобы передать пользователю сообщение об ошибке между запросами
        try
        {
            await _service.CreateAsync(originalUrl);
        }
        catch (Exception ex)
        {  
            TempData["Error"] = ex.Message;                                           
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)                                      // Открыть страницу редактирования конкретной ссылки
    {
        var link = await _repository.GetByIdAsync(id);
        if (link == null)
            return NotFound();

        return View(link);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, string originalUrl)                  //Для сохранения изменений длинного URL
    {
        var link = await _repository.GetByIdAsync(id);
        if (link == null)
            return NotFound();

        try
        {
            UrlValidator.Validate(originalUrl);
            link.Update(originalUrl);
            await _repository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return View(link);
        }

        return RedirectToAction(nameof(Index));                                         // RedirectToAction используем PRG pattern для предотвращения повторной отправки формы
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)                                    //Удаление короткой ссылки
    {
        var link = await _repository.GetByIdAsync(id);
        if (link != null)
        {
            await _repository.DeleteAsync(link);
            await _repository.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
