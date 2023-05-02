using System.Security.Claims;
using Application.Examples.Queries.GetExampleDetails;
using BlazorServerApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

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
    public IEnumerable<Claim>? Claims;


    public static ExampleDetailsDto? Example { get; set; } = new ExampleDetailsDto
    {
        ExampleFull = "He WALKEDOUT ON his wife last year.",
        //ExampleFullUnderscore = "___ _____ her and give her the message before she leaves the building.",
        //ExampleId = Guid.NewGuid(),
        Id = 1,
        ExampleParticle = "OUT ON",
        ExampleVerb = "WALKED",
        Meaning = "Chase.",
        Verb = "Get",
        VerbAndParticle = "Get after"
    };

    protected override async Task OnInitializedAsync()
    {
        await InitializeUser();
        await InitializeSessionProperties();
        
        CheckButtonTitle = $"Check ({AttemptsLeft})";
        try
        {
            Example = await ExampleDetailsService.GetRandomExampleDetails();
        }
        catch (Exception ex)
        {
            Console.WriteLine("##########" + Example?.ExampleFull);
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

    private async Task InitializeUser()
    {
        Claims = HttpContextAccessor.HttpContext?.User.Claims;
    }

    private void ParseDto()
    {
        Answer = Example.ExampleVerb + " " + Example.ExampleParticle;
        var tempAnswer = Example.ExampleFull;
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