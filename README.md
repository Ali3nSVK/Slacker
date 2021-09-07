# Slacker
[![.NET Framework](https://github.com/Ali3nSVK/Slacker/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/Ali3nSVK/Slacker/actions/workflows/dotnet-desktop.yml)

An app to prevent PC from going to sleep while AFK. Can be used to force Online/Available status on communication programs like Teams. Simulates key strokes on set interval.
Time interval and key pressed can be set, as well as the ability to use only KeyUp or full KeyPress events.

By default invokes F15 KeyUp every 59 seconds.

Uses simple WPF UI, Threads, WinForms Notification Icon, Timers, Registry to save settings and hooks Win32 DLL for WinAPI calls.
