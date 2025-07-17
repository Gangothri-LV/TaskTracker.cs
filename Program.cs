using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TaskTracker
{
    class Program
    {
        static string usersFile = "users.csv";
        static string tasksFile = "tasks.csv";
        static string loggedInUser = null;

        static void Main(string[] args)
        {
            Console.WriteLine("=== Welcome to Task Tracker ===");

            while (loggedInUser == null)
            {
                Console.WriteLine("\n1. Login");
                Console.WriteLine("2. Register");
                Console.Write("Choose: ");
                string option = Console.ReadLine();

                if (option == "1") Login();
                else if (option == "2") Register();
                else Console.WriteLine("Invalid choice.");
            }

            while (true)
            {
                Console.WriteLine($"\n=== Task Menu ({loggedInUser}) ===");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. View Tasks");
                Console.WriteLine("3. Mark Task Completed");
                Console.WriteLine("4. Delete Task");
                Console.WriteLine("5. Logout");
                Console.Write("Choose: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddTask(); break;
                    case "2": ViewTasks(); break;
                    case "3": MarkTaskCompleted(); break;
                    case "4": DeleteTask(); break;
                    case "5": return;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }

        static void Register()
        {
            Console.Write("Enter new username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            if (File.Exists(usersFile))
            {
                var users = File.ReadAllLines(usersFile);
                if (users.Any(u => u.Split(',')[0] == username))
                {
                    Console.WriteLine("User already exists.");
                    return;
                }
            }

            File.AppendAllText(usersFile, $"{username},{password}\n");
            Console.WriteLine("User registered successfully!");
        }

        static void Login()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();

            if (File.Exists(usersFile))
            {
                var users = File.ReadAllLines(usersFile);
                if (users.Any(u => u == $"{username},{password}"))
                {
                    loggedInUser = username;
                    Console.WriteLine("Login successful!");
                }
                else
                {
                    Console.WriteLine("Invalid credentials.");
                }
            }
        }

        static void AddTask()
        {
            Console.Write("Task Title: ");
            string title = Console.ReadLine();
            Console.Write("Description: ");
            string desc = Console.ReadLine();

            File.AppendAllText(tasksFile, $"{loggedInUser},{title},{desc},false\n");
            Console.WriteLine("Task added.");
        }

        static void ViewTasks()
        {
            if (!File.Exists(tasksFile))
            {
                Console.WriteLine("No tasks yet.");
                return;
            }

            var tasks = File.ReadAllLines(tasksFile)
                .Select(line => line.Split(','))
                .Where(parts => parts[0] == loggedInUser)
                .ToList();

            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks for this user.");
                return;
            }

            Console.WriteLine("\n--- Your Tasks ---");
            for (int i = 0; i < tasks.Count; i++)
            {
                var t = tasks[i];
                Console.WriteLine($"{i + 1}. Title: {t[1]}, Desc: {t[2]}, Done: {t[3]}");
            }
        }

        static void MarkTaskCompleted()
        {
            var lines = File.ReadAllLines(tasksFile).ToList();
            var userTasks = lines
                .Select((line, index) => new { Line = line, Index = index, Parts = line.Split(',') })
                .Where(t => t.Parts[0] == loggedInUser)
                .ToList();

            if (userTasks.Count == 0)
            {
                Console.WriteLine("No tasks to update.");
                return;
            }

            ViewTasks();
            Console.Write("Enter task number to mark completed: ");
            int choice = int.Parse(Console.ReadLine());

            if (choice < 1 || choice > userTasks.Count)
            {
                Console.WriteLine("Invalid task number.");
                return;
            }

            var task = userTasks[choice - 1];
            task.Parts[3] = "true";
            lines[task.Index] = string.Join(",", task.Parts);
            File.WriteAllLines(tasksFile, lines);
            Console.WriteLine("Task marked completed.");
        }

        static void DeleteTask()
        {
            var lines = File.ReadAllLines(tasksFile).ToList();
            var userTasks = lines
                .Select((line, index) => new { Line = line, Index = index, Parts = line.Split(',') })
                .Where(t => t.Parts[0] == loggedInUser)
                .ToList();

            if (userTasks.Count == 0)
            {
                Console.WriteLine("No tasks to delete.");
                return;
            }

            ViewTasks();
            Console.Write("Enter task number to delete: ");
            int choice = int.Parse(Console.ReadLine());

            if (choice < 1 || choice > userTasks.Count)
            {
                Console.WriteLine("Invalid task number.");
                return;
            }

            lines.RemoveAt(userTasks[choice - 1].Index);
            File.WriteAllLines(tasksFile, lines);
            Console.WriteLine("Task deleted.");
        }
    }
}