using System;
using System.Collections.Generic;
using System.Linq;

namespace RIS_p2_LR3
{
    /// <summary>
    /// Handles user interface and menu operations
    /// </summary>
    public class UserInterface
    {
        private readonly DepartmentManager _departmentManager;

        public UserInterface()
        {
            _departmentManager = new DepartmentManager();
        }

        /// <summary>
        /// Main application loop
        /// </summary>
        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Система учета сотрудников в подразделениях ===");
            Console.WriteLine();

            while (true)
            {
                ShowMainMenu();
                var choice = GetUserChoice();

                switch (choice)
                {
                    case 1:
                        ShowAllDepartments();
                        break;
                    case 2:
                        AddDepartment();
                        break;
                    case 3:
                        UpdateDepartment();
                        break;
                    case 4:
                        DeleteDepartment();
                        break;
                    case 5:
                        SearchDepartments();
                        break;
                    case 6:
                        SortDepartments();
                        break;
                    case 7:
                        ShowStatistics();
                        break;
                    case 8:
                        CreateBackup();
                        break;
                    case 0:
                        Console.WriteLine("До свидания!");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        /// <summary>
        /// Shows main menu
        /// </summary>
        private void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("    СИСТЕМА УЧЕТА СОТРУДНИКОВ В ПОДРАЗДЕЛЕНИЯХ");
            Console.WriteLine("================================================");
            Console.WriteLine();
            Console.WriteLine("ГЛАВНОЕ МЕНЮ:");
            Console.WriteLine("1. Просмотр всех подразделений");
            Console.WriteLine("2. Добавить подразделение");
            Console.WriteLine("3. Изменить подразделение");
            Console.WriteLine("4. Удалить подразделение");
            Console.WriteLine("5. Поиск подразделений");
            Console.WriteLine("6. Сортировка подразделений");
            Console.WriteLine("7. Статистика");
            Console.WriteLine("8. Создать резервную копию");
            Console.WriteLine("0. Выход");
            Console.WriteLine();
            Console.Write("Выберите действие: ");
        }

        /// <summary>
        /// Gets user choice from console
        /// </summary>
        /// <returns>User choice as integer</returns>
        private int GetUserChoice()
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    return choice;
                }
                Console.Write("Введите корректное число: ");
            }
        }

        /// <summary>
        /// Shows all departments
        /// </summary>
        private void ShowAllDepartments()
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("              ВСЕ ПОДРАЗДЕЛЕНИЯ");
            Console.WriteLine("================================================");
            
            var departments = _departmentManager.GetAllDepartments();

            if (!departments.Any())
            {
                Console.WriteLine("\nПодразделения не найдены.");
                return;
            }

            Console.WriteLine($"\nНайдено подразделений: {departments.Count}");
            Console.WriteLine();

            for (int i = 0; i < departments.Count; i++)
            {
                var department = departments[i];
                
                Console.WriteLine($"ПОДРАЗДЕЛЕНИЕ #{department.Id}: {department.Name}");
                Console.WriteLine($"Описание: {department.Description}");
                Console.WriteLine($"Сотрудников: {department.EmployeeCount}");
                Console.WriteLine($"Создано: {department.CreatedDate:dd.MM.yyyy} | Изменено: {department.LastModifiedDate:dd.MM.yyyy}");
                
                if (i < departments.Count - 1)
                {
                    Console.WriteLine(new string('-', 50));
                }
            }
        }

        /// <summary>
        /// Adds new department
        /// </summary>
        private void AddDepartment()
        {
            Console.WriteLine("=== ДОБАВЛЕНИЕ ПОДРАЗДЕЛЕНИЯ ===");
            
            Console.Write("Название подразделения: ");
            string name = Console.ReadLine();

            Console.Write("Описание (необязательно): ");
            string description = Console.ReadLine();

            Console.Write("Количество сотрудников: ");
            if (!int.TryParse(Console.ReadLine(), out int employeeCount))
            {
                Console.WriteLine("Неверный формат количества сотрудников.");
                return;
            }

            if (_departmentManager.AddDepartment(name, description, employeeCount))
            {
                Console.WriteLine("\nПодразделение успешно добавлено!");
            }
        }

        /// <summary>
        /// Updates existing department
        /// </summary>
        private void UpdateDepartment()
        {
            Console.WriteLine("=== ИЗМЕНЕНИЕ ПОДРАЗДЕЛЕНИЯ ===");
            
            Console.Write("Введите ID подразделения для изменения: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный формат ID.");
                return;
            }

            var department = _departmentManager.GetAllDepartments().FirstOrDefault(d => d.Id == id);
            if (department == null)
            {
                Console.WriteLine("Подразделение с указанным ID не найдено.");
                return;
            }

            Console.WriteLine($"Текущие данные: {department.ToString()}");
            Console.WriteLine();

            Console.Write($"Название подразделения (текущее: {department.Name}): ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                name = department.Name;
            }

            Console.Write($"Описание (текущее: {department.Description}): ");
            string description = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(description))
            {
                description = department.Description;
            }

            Console.Write($"Количество сотрудников (текущее: {department.EmployeeCount}): ");
            string employeeCountInput = Console.ReadLine();
            int employeeCount = department.EmployeeCount;
            if (!string.IsNullOrWhiteSpace(employeeCountInput) && int.TryParse(employeeCountInput, out int newCount))
            {
                employeeCount = newCount;
            }

            if (_departmentManager.UpdateDepartment(id, name, description, employeeCount))
            {
                Console.WriteLine("\nПодразделение успешно обновлено!");
            }
        }

        /// <summary>
        /// Deletes department
        /// </summary>
        private void DeleteDepartment()
        {
            Console.WriteLine("=== УДАЛЕНИЕ ПОДРАЗДЕЛЕНИЯ ===");
            
            Console.Write("Введите ID подразделения для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный формат ID.");
                return;
            }

            var department = _departmentManager.GetAllDepartments().FirstOrDefault(d => d.Id == id);
            if (department == null)
            {
                Console.WriteLine("Подразделение с указанным ID не найдено.");
                return;
            }

            Console.WriteLine($"Вы действительно хотите удалить подразделение: {department.Name}? (y/n)");
            var confirmation = Console.ReadLine();
            if (confirmation?.ToLower() == "y" || confirmation?.ToLower() == "yes")
            {
                if (_departmentManager.DeleteDepartment(id))
                {
                    Console.WriteLine("\nПодразделение успешно удалено!");
                }
            }
            else
            {
                Console.WriteLine("\nУдаление отменено.");
            }
        }

        /// <summary>
        /// Searches departments
        /// </summary>
        private void SearchDepartments()
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("              ПОИСК ПОДРАЗДЕЛЕНИЙ");
            Console.WriteLine("================================================");
            Console.WriteLine();
            Console.Write("Введите поисковый запрос (название или описание): ");
            string searchTerm = Console.ReadLine();

            var results = _departmentManager.SearchDepartments(searchTerm);

            if (!results.Any())
            {
                Console.WriteLine("\nПодразделения не найдены.");
                return;
            }

            Console.WriteLine($"\nНайдено подразделений: {results.Count}");
            Console.WriteLine();

            for (int i = 0; i < results.Count; i++)
            {
                var department = results[i];
                
                Console.WriteLine($"РЕЗУЛЬТАТ #{i + 1}: ID {department.Id} - {department.Name}");
                Console.WriteLine($"Описание: {department.Description}");
                Console.WriteLine($"Сотрудников: {department.EmployeeCount}");
                Console.WriteLine($"Создано: {department.CreatedDate:dd.MM.yyyy} | Изменено: {department.LastModifiedDate:dd.MM.yyyy}");
                
                if (i < results.Count - 1)
                {
                    Console.WriteLine(new string('-', 50));
                }
            }
        }

        /// <summary>
        /// Sorts departments
        /// </summary>
        private void SortDepartments()
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("            СОРТИРОВКА ПОДРАЗДЕЛЕНИЙ");
            Console.WriteLine("================================================");
            Console.WriteLine();
            Console.WriteLine("КРИТЕРИЙ СОРТИРОВКИ:");
            Console.WriteLine("1. По названию");
            Console.WriteLine("2. По количеству сотрудников");
            Console.WriteLine("3. По дате создания");
            Console.WriteLine("4. По дате изменения");
            Console.Write("Ваш выбор: ");

            if (!int.TryParse(Console.ReadLine(), out int sortChoice) || sortChoice < 1 || sortChoice > 4)
            {
                Console.WriteLine("\nНеверный выбор.");
                return;
            }

            Console.WriteLine("\nПОРЯДОК СОРТИРОВКИ:");
            Console.WriteLine("1. По возрастанию");
            Console.WriteLine("2. По убыванию");
            Console.Write("Ваш выбор: ");

            if (!int.TryParse(Console.ReadLine(), out int orderChoice) || orderChoice < 1 || orderChoice > 2)
            {
                Console.WriteLine("\nНеверный выбор.");
                return;
            }

            string[] sortCriteria = { "", "name", "employees", "created", "modified" };
            string[] sortNames = { "", "названию", "количеству сотрудников", "дате создания", "дате изменения" };
            bool ascending = orderChoice == 1;

            var sortedDepartments = _departmentManager.SortDepartments(sortCriteria[sortChoice], ascending);

            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("         ОТСОРТИРОВАННЫЕ ПОДРАЗДЕЛЕНИЯ");
            Console.WriteLine("================================================");
            Console.WriteLine($"Сортировка по {sortNames[sortChoice]} ({(ascending ? "по возрастанию" : "по убыванию")})");
            Console.WriteLine();

            for (int i = 0; i < sortedDepartments.Count; i++)
            {
                var department = sortedDepartments[i];
                
                Console.WriteLine($"#{i + 1}: ID {department.Id} - {department.Name}");
                Console.WriteLine($"Описание: {department.Description}");
                Console.WriteLine($"Сотрудников: {department.EmployeeCount}");
                Console.WriteLine($"Создано: {department.CreatedDate:dd.MM.yyyy} | Изменено: {department.LastModifiedDate:dd.MM.yyyy}");
                
                if (i < sortedDepartments.Count - 1)
                {
                    Console.WriteLine(new string('-', 50));
                }
            }
        }

        /// <summary>
        /// Shows statistics
        /// </summary>
        private void ShowStatistics()
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("                  СТАТИСТИКА");
            Console.WriteLine("================================================");
            
            var departments = _departmentManager.GetAllDepartments();
            if (!departments.Any())
            {
                Console.WriteLine("\nНет данных для статистики.");
                return;
            }

            var totalDepartments = departments.Count;
            var totalEmployees = departments.Sum(d => d.EmployeeCount);
            var avgEmployees = totalEmployees / (double)totalDepartments;
            var maxEmployees = departments.Max(d => d.EmployeeCount);
            var minEmployees = departments.Min(d => d.EmployeeCount);
            var maxDept = departments.First(d => d.EmployeeCount == maxEmployees);
            var minDept = departments.First(d => d.EmployeeCount == minEmployees);

            Console.WriteLine();
            Console.WriteLine("ОБЩАЯ СТАТИСТИКА:");
            Console.WriteLine($"Общее количество подразделений: {totalDepartments}");
            Console.WriteLine($"Общее количество сотрудников: {totalEmployees}");
            Console.WriteLine($"Среднее количество сотрудников: {avgEmployees:F1}");
            Console.WriteLine();
            Console.WriteLine("ЭКСТРЕМАЛЬНЫЕ ЗНАЧЕНИЯ:");
            Console.WriteLine($"Максимальное количество: {maxEmployees} ({maxDept.Name})");
            Console.WriteLine($"Минимальное количество: {minEmployees} ({minDept.Name})");
        }

        /// <summary>
        /// Creates backup
        /// </summary>
        private void CreateBackup()
        {
            Console.WriteLine("=== СОЗДАНИЕ РЕЗЕРВНОЙ КОПИИ ===");
            if (_departmentManager.CreateBackup())
            {
                Console.WriteLine("Резервная копия успешно создана.");
            }
            else
            {
                Console.WriteLine("Ошибка при создании резервной копии.");
            }
        }
    }
}
