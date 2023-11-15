using ConsoleTools;

namespace CodeSlackers.HostedConsole;

public static class MenuManager
{
    public static ConsoleMenu YesNo(Action yes, Action no, string prompt)
    {
        var menu = new ConsoleMenu();
        menu.Add("Yes", () =>
        {
            menu.CloseMenu();
            yes();
        });
        menu.Add("No", () =>
        {
            menu.CloseMenu();
            no();
        }).Configure(config => config.WriteHeaderAction = () => Console.WriteLine(prompt));

        return menu;
    }

    public static ConsoleMenu AnswerQuestion(
        List<string> questions,
        Action<ConsoleMenu, string> answerAction,
        string prompt)
    {
        var menu = new ConsoleMenu();
        foreach (var question in questions)
        {
            menu.Add(question, () => answerAction(menu, question));
        }
        menu.Configure(config => config.WriteHeaderAction = () => Console.WriteLine(prompt));
        return menu;
    }

    public static ConsoleMenu SelectEnum<T>(Action<T, ConsoleMenu> selectEnum, string prompt) where T : struct, Enum
    {
        var questions = Enum.GetNames<T>().ToList();
        var menu = new ConsoleMenu();
        foreach (var question in questions)
        {
            menu.Add(question, () => selectEnum(Enum.Parse<T>(question), menu));
        }

        menu.Configure(config => config.WriteHeaderAction = () => Console.WriteLine(prompt));
        return menu;
    }

}