using Application.Examples.Queries.GetExampleDetails;
using BlazorServerApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorServerApp.Pages;

[Authorize]
public class PlayGameBase : ComponentBase
{
    public string Answer { get; set; } = string.Empty;
    public List<string> AnswerList { get; set; } = new();
    public List<string> SentenceList { get; set; } = new ();
    public bool result;
    public ElementReference[] inputRefs;
    public string[] inputLetters = Array.Empty<string>();
    public ElementReference invisibleInput;
    [Inject] public IExampleDetailsService ExampleDetailsService { get; set; }
    public string ErrorMessage { get; set; }
    
    public static ExampleDetailsDto? Example { get; set; } = new ExampleDetailsDto
    {
        ExampleFull = "I DRIED the dishes UP.",
        ExampleFullUnderscore = "I _____ the dishes __.",
        ExampleId = Guid.NewGuid(),
        ExampleParticle = "UP",
        ExampleVerb = "DRIED",
        Meaning = "Dry plates, dishes, cutlery, etc, after washing them up.",
        Verb = "Dry",
        VerbAndParticle = "Dry up"
    };

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Example = await ExampleDetailsService.GetRandomExampleDetails();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        
        Answer = Example.ExampleVerb + " " + Example.ExampleParticle;
        var tempAnswer = Example.ExampleFull;
        AnswerList = Answer.Split(" ").ToList();
            
        var temp = new List<string>();
        foreach (var item in AnswerList)
        {
            temp = tempAnswer.Split(" " + item).ToList();
            SentenceList.Add(temp[0]);
            tempAnswer = string.Join("", temp[1]);
        }
        SentenceList.Add(tempAnswer);
        //SentenceList.Add(temp[1]);

        var letterCount = AnswerList.Sum(str => str.Length);
        inputRefs = new ElementReference[letterCount];
        inputLetters = new string[letterCount];
    }

    public void CheckExample()
    {
        result = string.Join("", inputLetters)
            .Equals(string.Join("", AnswerList), StringComparison.InvariantCultureIgnoreCase);
    }


    public void HandleInput(int index, ChangeEventArgs e)
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

    public void HandleKeyDown(int index, KeyboardEventArgs e)
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

    [Inject] private IJSRuntime JSRuntime { get; set; }

    public async Task HandleFocusIn(int index)
    {
        await JSRuntime.InvokeVoidAsync("eval", @"
            function setTextSelection(element, start, end) {
                element.setSelectionRange(start, end);
            }
        ");

        await JSRuntime.InvokeVoidAsync("setTextSelection", inputRefs[index], 0, -1);
    }
}