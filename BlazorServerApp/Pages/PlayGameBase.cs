using Application.Examples.Queries.GetExampleDetails;
using BlazorServerApp.Models;
using BlazorServerApp.Services.IServices;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace BlazorServerApp.Pages;

public class PlayGameBase : ComponentBase
{
    public string Answer { get; set; } = string.Empty;
    public List<string> AnswerList { get; set; } = new();
    public string[] AnswerLetterArray { get; set; }
    public List<string> SentenceList { get; set; } = new();
    public ElementReference[] InputRefs;
    public string[] EnteredLetters = Array.Empty<string>();
    public ElementReference InvisibleInput;
    [Inject] public IExampleDetailsService ExampleDetailsService { get; set; }
    public List<string> BorderColors { get; set; } = new();
    public int AttemptsLeft = 3;
    public string ErrorMessage { get; set; }
    [Inject] private IJSRuntime JSRuntime { get; set; }
    public bool IsRoundOver;
    public string CheckButtonTitle = "";
    [Inject] private NavigationManager NavigationManager { get; set; }

    [Inject] public SessionProperties SessionProperties { get; set; }
    [Inject] public IHttpContextAccessor HttpContextAccessor { get; set; }
    [Inject] public ProtectedSessionStorage ProtectedSessionStore { get; set; }

    public static ExampleDetailsDto? Example { get; set; } = new ExampleDetailsDto
    {
        Id = 448,
        Body = "She thought he had always been faithful to her, but he had been CHEATING ON her ever since their wedding day (with one of the bridesmaids).",
        VerbId = 117,
        Verb = new Verb
        {
            Id = 117,
            Body = "Cheat",
            Productivity = 1
        },
        ParticleId = 3,
        Particle = new Particle
        {
            Id = 3,
            Body = "on",
            Frequency = 1086
        },
        MeaningId = 423,
        Meaning = new Meaning
        {
            Id = 423,
            Body = "Leave a place angrily."
        },
        PhrasalVerbId = 314,
        PhrasalVerb = new PhrasalVerb
        {
            Id = 314,
            Body = "Cheat on",
            VerbId = 690,
            ParticleId = 4,
            Frequency = 1
        },
        ExampleVerb = "CHEATING"
    };

    protected override async Task OnInitializedAsync()
    {
        await InitializeSessionProperties();

        CheckButtonTitle = $"Check ({AttemptsLeft})";
        try
        {
            var response = await ExampleDetailsService.GetRandomExampleDetails<ResponseDto>();
            Example = JsonConvert.DeserializeObject<ExampleDetailsDto>(Convert.ToString(response.Result));
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        ParseDto();

        if (SessionProperties.IsShowVerb)
        {
            for (var i = 0; i < AnswerList[0].Length; i++)
            {
                EnteredLetters[i] = AnswerList[0][i].ToString();
            }
        }

        if (SessionProperties.IsShowParticles)
        {
            ShowParticles();
        }
    }

    private async Task InitializeSessionProperties()
    {
        var result = await ProtectedSessionStore.GetAsync<SessionProperties>("sessionProperties");
        if (result.Success)
        {
            SessionProperties.IsShowVerb = result.Value.IsShowVerb;
            SessionProperties.IsShowParticles = result.Value.IsShowParticles;
        }
    }

    private void ParseDto()
    {
        Answer = Example.ExampleVerb + " " + Example.Particle.Body.ToUpper();
        var tempAnswer = Example.Body;
        AnswerList = Answer.Split(" ").ToList();

        foreach (var item in AnswerList)
        {
            var temp = tempAnswer.Split(" " + item).ToList();

            if (temp.Count == 1)
                temp = tempAnswer.Split(item).ToList();

            SentenceList.Add(temp[0]);
            tempAnswer = string.Join("", temp[1]);
        }
        SentenceList.Add(tempAnswer);

        var letterCount = AnswerList.Sum(str => str.Length);
        InputRefs = new ElementReference[letterCount];
        EnteredLetters = new string[letterCount];
        SetAnswerLetterArray(letterCount);
        AddDefaultBorderStyle(letterCount);
    }

    private void AddDefaultBorderStyle(int letterCount)
    {
        for (var i = 0; i < letterCount; i++)
            BorderColors.Add("border: 1px solid #ced4da;");
    }

    private void SetAnswerLetterArray(int letterCount)
    {
        AnswerLetterArray = new string[letterCount];
        var temp2 = string.Join("", AnswerList);
        for (var i = 0; i < letterCount; i++)
            AnswerLetterArray[i] = temp2[i].ToString();
    }

    public async Task CheckAnswer()
    {
        if (IsRoundOver) return;
        var enteredLetters = string.Join("", EnteredLetters);
        if (enteredLetters.Length == 0) return;

        AttemptsLeft--;
        CheckButtonTitle = $"Check ({AttemptsLeft})";

        for (var i = 0; i < AnswerLetterArray.Length; i++)
        {
            if (!EnteredLetters[i].IsNullOrEmpty())
            {
                BorderColors[i] = AnswerLetterArray[i]
                    .Equals(EnteredLetters[i], StringComparison.InvariantCultureIgnoreCase)
                    ? "border: 1px solid #008000;"
                    : "border: 1px solid #FF0000;";
            }
        }

        if (enteredLetters.Equals(string.Join("", AnswerList), StringComparison.InvariantCultureIgnoreCase))
        {
            CheckButtonTitle = "Correct! Well Done!";
            IsRoundOver = true;
            return;
        }

        if (AttemptsLeft == 0)
        {
            IsRoundOver = true;
            /*CheckButtonTitle =
                enteredLetters.Equals(string.Join("", AnswerList), StringComparison.InvariantCultureIgnoreCase)
                    ? "Right!"
                    : "Wrong";*/
            CheckButtonTitle = "Oops. Good like next time!";
        }
    }

    public void GoNextRound()
    {
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }

    public void ShowParticles()
    {
        var startIndex = AnswerList[0].Length;
        var answer = string.Join("", AnswerList);
        for (var i = startIndex; i < answer.Length; i++)
            EnteredLetters[i] = answer[i].ToString();
    }

    public void ShowVerbNextLetter()
    {
        for (var i = 0; i < AnswerList[0].Length; i++)
        {
            if (EnteredLetters[i].IsNullOrEmpty())
            {
                EnteredLetters[i] = AnswerList[0][i].ToString();
                return;
            }
        }
    }

    public async Task ChangeShowVerbChecker()
    {
        SessionProperties.IsShowVerb = !SessionProperties.IsShowVerb;
        await ProtectedSessionStore.SetAsync("sessionProperties", SessionProperties);
    }

    public async Task ChangeShowParticlesChecker()
    {
        SessionProperties.IsShowParticles = !SessionProperties.IsShowParticles;
        await ProtectedSessionStore.SetAsync("sessionProperties", SessionProperties);
    }

    public void HandleInput(int index, ChangeEventArgs e)
    {
        var key = e.Value?.ToString();

        if (key == "")
        {
            InvisibleInput.FocusAsync();
            InputRefs[index].FocusAsync();
            return;
        }

        if (index + 1 < InputRefs.Length)
        {
            InputRefs[index + 1].FocusAsync();
        }
        else
        {
            InvisibleInput.FocusAsync();
            InputRefs[index].FocusAsync();
        }
    }

    public void HandleKeyDown(int index, KeyboardEventArgs e)
    {
        switch (e.Key)
        {
            case "ArrowLeft" when index - 1 >= 0:
                InputRefs[index - 1].FocusAsync();
                break;
            case "ArrowRight" when index + 1 < InputRefs.Length:
                InputRefs[index + 1].FocusAsync();
                break;
            //case "Backspace" when string.IsNullOrEmpty(EnteredLetters[index]) && index - 1 >= 0:
            case "Backspace" when index - 1 >= 0:
                InputRefs[index - 1].FocusAsync();
                break;
        }
    }

    public async Task HandleFocusIn(int index)
    {
        await JSRuntime.InvokeVoidAsync("eval", @"
            function setTextSelection(element, start, end) {
                element.setSelectionRange(start, end);
            }
        ");

        await JSRuntime.InvokeVoidAsync("setTextSelection", InputRefs[index], 0, -1);
    }
}