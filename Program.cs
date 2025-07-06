using System.ComponentModel.Design;
using Spectre.Console;
using System.Collections.Generic;
using ToDo_App;

internal class Program
{
    private static void Main(string[] args)
    {
        ToDo_List list = new ToDo_List(); // список
        list = FileHandler.LoadFromFile("tasks.json");

        MenuAction act = MenuAction.Default; // действие с меню

        Console.WriteLine("TO-DO LIST\n090304-РПИб-о23 Рыжкова Е.А.\n\n");

        do
        {
            ShowList(list);
            act = Menu();
            switch (act)
            {
                case MenuAction.AddTask:
                    {
                        // Добавление задачи
                        AddTask(list);
                        break;
                    }
                case MenuAction.RemoveTask:
                    {
                        // Удаление задачи
                        DeleteTask(list);
                        break;
                    }
                case MenuAction.ChangeStatus:
                    {
                        // Изменить статус задачи
                        ChangeStatusTask(list);
                        break;
                    }
                case MenuAction.SaveFile:
                    {
                        // Сохранение в файл
                        SaveToFile(list);
                        break;
                    }
                case MenuAction.SortList:
                    {
                        // Вывести отсортированный список
                        list.SortLists();
                        break;
                    }
                case MenuAction.CloseProgram:
                    {
                        break;
                    }
            }
        }
        while (act != MenuAction.CloseProgram); // выполнять, пока не будет команды "Закрыть программу"

        Console.ReadKey();
    }

    // меню приложения
    public static MenuAction Menu()
    {
        int otvet = 0; // ответ пользователя
        MenuAction action = MenuAction.Default;

        Console.WriteLine("Выберите действие:\n\n1. Добавить задачу\n" +
            "2. Удалить задачу\n" +
            "3. Сохранить список в файл\n" +
            "4. Изменить статус задачи\n" +
            "5. Отсортировать список по приоритету\n" +
            "0. Закрыть программу\n");
        Console.Write("Введите ответ: ");
        int.TryParse(Console.ReadLine(), out otvet);

        switch (otvet)
        {
            case 1:
                {
                    action = MenuAction.AddTask;
                    break;
                }
            case 2: { 
                    action = MenuAction.RemoveTask;
                    break;
                }
            case 3:
                {
                    action = MenuAction.SaveFile;
                    break;
                }
            case 4: {
                    action = MenuAction.ChangeStatus;
                    break;
                }
            case 5:
                {
                    action = MenuAction.SortList;
                    break;
                }
            case 0:
                {
                    action = MenuAction.CloseProgram;
                    break;
                }
        }

        return action;
    }

    // Показать список
    public static void ShowList(ToDo_List list)
    {
        // если список пуст
        if (list.ListCount == 0)
        {
            Console.WriteLine("Список задач пуст!");
        }
        else {
            var showTable = new Table()
                .Border(TableBorder.Simple)
                .Title("ToDo List");

            showTable.AddColumn(new TableColumn("№").Centered())
                .AddColumn(new TableColumn("Статус").Centered())
                .AddColumn(new TableColumn("Задача"))
                .AddColumn(new TableColumn("Приоритет"));

            // сначала отобразим невыполненные задачи
            int number = 0;
            if(list.ListNotCompleted.Count != 0)
            {
                foreach (var item in list.ListNotCompleted)
                {
                    number++;
                    string status = item.Status ? "[green]+[/]" : "[grey]-[/]";
                    string prioritet = "";

                    switch (item.Prioritet)
                    {
                        case Priority.Default:
                            {
                                prioritet = "";
                                break;
                            }
                        case Priority.Low:
                            {
                                prioritet = "Низкий";
                                break;
                            }
                        case Priority.Normal:
                            {
                                prioritet = "Средний";
                                break;
                            }
                        case Priority.High:
                            {
                                prioritet = "Высокий";
                                break;
                            }
                    }

                    showTable.AddRow((number).ToString(), status, item.TextTask, prioritet);
                }
            }

            // потом выполненные задачи
            if (list.ListCompleted.Count != 0)
            {
                foreach (var item in list.ListCompleted)
                {
                    number++;
                    string status = item.Status ? "[green]+[/]" : "[grey]-[/]";
                    string prioritet = "";

                    switch (item.Prioritet)
                    {
                        case Priority.Default:
                            {
                                prioritet = "";
                                break;
                            }
                        case Priority.Low:
                            {
                                prioritet = "Низкий";
                                break;
                            }
                        case Priority.Normal:
                            {
                                prioritet = "Средний";
                                break;
                            }
                        case Priority.High:
                            {
                                prioritet = "Высокий";
                                break;
                            }
                    }

                    showTable.AddRow((number).ToString(), status, item.TextTask, prioritet);
                }
            }
            
            AnsiConsole.Write(showTable);
        }
    }

    // Добавление в список задачу
    public static void AddTask(ToDo_List list)
    {
        string textTask = null;
        int otv = 0;
        Priority prioritet = Priority.Default;

        Console.Write("Введите текст задачи: ");
        textTask = Console.ReadLine();

        Console.WriteLine("Введите приоритет выполнения:\n0 - по умолчанию\n1 - низкий\n2 - средний\n3 - высокий");
        int.TryParse(Console.ReadLine(), out otv);
        switch (otv)
        {
            case 0:
                {
                    prioritet = Priority.Default;
                    break;
                }
            case 1:
                {
                    prioritet = Priority.Low;
                    break;
                }
            case 2:
                {
                    prioritet = Priority.Normal;
                    break;
                }
            case 3:
                {
                    prioritet = Priority.High;
                    break;
                }
        }

        list.AddNewTask(new ToDo_Task(textTask, prioritet));
    }

    // Удалить задачу из списка
    public static void DeleteTask(ToDo_List list)
    {
        // если список пуст
        if (list.ListCount == 0)
        {
            Console.WriteLine("Список задач пуст!");
        }
        else
        {
            int otvet = 0;
            Console.Write("Введите номер задачи: ");
            if (int.TryParse(Console.ReadLine(), out otvet))
            {
                if (otvet > list.ListCount)
                {
                    Console.WriteLine("Этой задачи нет!");
                }
                else
                {
                    list.RemoveTask(otvet);
                }
            }
            else
            {
                Console.WriteLine("Не понимаю!");
            }
        }
    }

    // Изменить статус задачи
    public static void ChangeStatusTask(ToDo_List list) {
        // если список пуст
        if (list.ListCount == 0)
        {
            Console.WriteLine("Список задач пуст!");
        }
        else
        {
            int otvet = 0;
            Console.Write("Введите номер задачи: ");
            if (int.TryParse(Console.ReadLine(),out otvet))
            {
                if (otvet > list.ListCount)
                {
                    Console.WriteLine("Этой задачи нет!");
                }
                else
                {
                    list.ChangeStatus(otvet);
                }
            }
            else
            {
                Console.WriteLine("Не понимаю!");
            }

            
        }
    }

    public static void SaveToFile(ToDo_List list)
    {
        FileHandler.SaveToFile("tasks.json", list);
        Console.WriteLine("Сохранение завершено!");
    }
}