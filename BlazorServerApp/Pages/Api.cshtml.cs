using System.Net.Http.Headers;
using Application.Examples.Queries.GetExampleDetails;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace BlazorServerApp.Pages;

[Authorize]
public class ApiModel : PageModel
{
    public ExampleDetailsDto? Example { get; set; } = new();
    private IHttpClientFactory HttpClientFactory { get; }
    [Inject]
    private IJSRuntime JsRuntime { get; set; }
    public string Answer { get; set; } = string.Empty;
    public List<string> AnswerList { get; set; } = new();
    public List<string> SentenceList { get; set; } = new();
    public bool result;
    public ElementReference[] inputRefs;
    public string[] inputLetters;
    public ElementReference invisibleInput;

    public ApiModel(IHttpClientFactory httpClientFactory)
    {
        HttpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        using var httpClient = HttpClientFactory.CreateClient();

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", await HttpContext.GetTokenAsync("access_token"));

        var dto = await httpClient.GetStringAsync("https://api:7001/api/Example/GetRandomExampleDetails");

        Example = JsonConvert.DeserializeObject<ExampleDetailsDto>(dto);

        Answer = Example.ExampleVerb + " " + Example.ExampleParticle;
        var tempAnswer = Example.ExampleFull;
        AnswerList = Answer.Split(" ").ToList();
        var temp = new List<string>();
        foreach (var item in AnswerList)
        {
            temp = tempAnswer.Split(item).ToList();
            SentenceList.Add(temp[0]);
            tempAnswer = string.Join("", temp[1]);
        }
        SentenceList.Add(temp[0]);
        SentenceList.Add(temp[1]);

        var letterCount = AnswerList.Sum(str => str.Length);
        inputRefs = new ElementReference[letterCount];
        inputLetters = new string[inputRefs.Length];
    }

    private void CheckExample()
    {
        result = string.Join("", inputLetters)
            .Equals(string.Join("", AnswerList), StringComparison.InvariantCultureIgnoreCase);
    }

    public void HandleInput(int index, ChangeEventArgs? e)
    {
        var key = e.Value?.ToString();

        if (key == "")
        {
            invisibleInput.FocusAsync();
            inputRefs[index].FocusAsync();
            return;
        }

        if (index + 1 < inputRefs.Length)
        {
            inputRefs[index + 1].FocusAsync();
        }
        else
        {
            invisibleInput.FocusAsync();
            inputRefs[index].FocusAsync();
        }
    }

    public void HandleKeyDown(int index, KeyboardEventArgs? e)
    {
        switch (e.Key)
        {
            case "ArrowLeft" when index - 1 >= 0:
                inputRefs[index - 1].FocusAsync();
                break;
            case "ArrowRight" when index + 1 < inputRefs.Length:
                inputRefs[index + 1].FocusAsync();
                break;
            case "Backspace" when string.IsNullOrEmpty(inputLetters[index]) && index - 1 >= 0:
                inputRefs[index - 1].FocusAsync();
                break;
        }
    }


    public async Task HandleFocusIn(int index)
    {
        await JsRuntime.InvokeVoidAsync("eval", @"
            function setTextSelection(element, start, end) {
                element.setSelectionRange(start, end);
            }
        ");

        await JsRuntime.InvokeVoidAsync("setTextSelection", inputRefs[index], 0, -1);
    }
}