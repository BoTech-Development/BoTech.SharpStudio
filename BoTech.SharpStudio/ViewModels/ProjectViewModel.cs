using Avalonia;
using Avalonia.Media;
using BoTech.SharpStudio.CSharpEngine.Models;
using ReactiveUI;

namespace BoTech.SharpStudio.ViewModels;

public class ProjectViewModel (Project project) : ViewModelBase
{
    public Project Project { get; } = project;
    public string ProjectName { get; set; } = project.Name;
    public string ProjectShortName => GetProjectShortName();
    public IBrush ProjectColor { get; set; }
    public IBrush ProjectBackgroundGradient { get; set; }
    
    private Color _firstGradientColor = Colors.White;
    public Color FirstGradientColor
    {
        get => _firstGradientColor; 
        set  
        {
            _firstGradientColor = value;
            ProjectBackgroundGradient = CreateGradientBrush();
        }
    }
    private Color _secondGradientColor = Colors.White;

    public Color SecondGradientColor
    {
        get => _secondGradientColor; 
        set 
        {
            _secondGradientColor = value;
            ProjectBackgroundGradient = CreateGradientBrush();
        }
    }
    private Color _thirdGradientColor = Colors.White;

    public Color ThirdGradientColor
    {
        get => _thirdGradientColor; 
        set
        {
            _thirdGradientColor = value;
            ProjectBackgroundGradient = CreateGradientBrush();
        }
    }
    private Color _fourthGradientColor = Colors.White;
    public Color FourthGradientColor
    {
        get => _fourthGradientColor; 
        set
        {
            _fourthGradientColor = value;
            ProjectBackgroundGradient = CreateGradientBrush();
        }
    }

    private LinearGradientBrush CreateGradientBrush()
    {
        return new LinearGradientBrush()
        {
            StartPoint = new RelativePoint(new Point(0, 0), RelativeUnit.Relative),
            EndPoint = new RelativePoint(new Point(100, 100), RelativeUnit.Relative),
            GradientStops = new GradientStops()
            {
                new GradientStop() { Color = FirstGradientColor, Offset = 0 },
                new GradientStop() { Color = SecondGradientColor, Offset = 0 },
                new GradientStop() { Color = ThirdGradientColor, Offset = 0 },
                new GradientStop() { Color = FourthGradientColor, Offset = 0 },
            }
        };
    }
    public string SubInfo { get; set; }

    private string GetProjectShortName()
    {
        string shortName = string.Empty;
        foreach (char character in ProjectName.ToCharArray())
        {
            if (char.IsLetter(character) && char.IsUpper(character))
            {
                shortName += character.ToString();
            }
        }

        if (shortName == string.Empty)
        {
            shortName = ProjectName[0].ToString();
        }
        return shortName;
    }
}