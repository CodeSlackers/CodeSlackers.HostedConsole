﻿using ConsoleTools;

namespace CodeSlackers.HostedConsole;

public interface IFlowIoService
{
    void WriteLine(string line);
    ConsoleKeyInfo ReadKey();
    string? ReadLine();
    void ShowMenu(ConsoleMenu menu);
    void CloseMenu(ConsoleMenu menu);
    void Clear();
}