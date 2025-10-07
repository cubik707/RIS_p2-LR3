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
            Console.WriteLine("=== ГЛАВНОЕ МЕНЮ ===");
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
            Console.WriteLine("=== ВСЕ ПОДРАЗДЕЛЕНИЯ ===");
            var departments = _departmentManager.GetAllDepartments();

            if (!departments.Any())
            {
                Console.WriteLine("Подразделения не найдены.");
                return;
            }

            Console.WriteLine($"Найдено подразделений: {departments.Count}");
            Console.WriteLine();

            foreach (var department in departments)
            {
                Console.WriteLine(department.ToString());
                Console.WriteLine(new string('-', 80));
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

            _departmentManager.AddDepartment(name, description, employeeCount);
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

            _departmentManager.UpdateDepartment(id, name, description, employeeCount);
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
                _departmentManager.DeleteDepartment(id);
            }
            else
            {
                Console.WriteLine("Удаление отменено.");
            }
        }

        /// <summary>
        /// Searches departments
        /// </summary>
        private void SearchDepartments()
        {
            Console.WriteLine("=== ПОИСК ПОДРАЗДЕЛЕНИЙ ===");
            Console.Write("Введите поисковый запрос (название или описание): ");
            string searchTerm = Console.ReadLine();

            var results = _departmentManager.SearchDepartments(searchTerm);

            if (!results.Any())
            {
                Console.WriteLine("Подразделения не найдены.");
                return;
            }

            Console.WriteLine($"Найдено подразделений: {results.Count}");
            Console.WriteLine();

            foreach (var department in results)
            {
                Console.WriteLine(department.ToString());
                Console.WriteLine(new string('-', 80));
            }
        }

        /// <summary>
        /// Sorts departments
        /// </summary>
        private void SortDepartments()
        {
            Console.WriteLine("=== СОРТИРОВКА ПОДРАЗДЕЛЕНИЙ ===");
            Console.WriteLine("Выберите критерий сортировки:");
            Console.WriteLine("1. По названию");
            Console.WriteLine("2. По количеству сотрудников");
            Console.WriteLine("3. По дате создания");
            Console.WriteLine("4. По дате изменения");
            Console.Write("Ваш выбор: ");

            if (!int.TryParse(Console.ReadLine(), out int sortChoice) || sortChoice < 1 || sortChoice > 4)
            {
                Console.WriteLine("Неверный выбор.");
                return;
            }

            Console.WriteLine("Выберите порядок сортировки:");
            Console.WriteLine("1. По возрастанию");
            Console.WriteLine("2. По убыванию");
            Console.Write("Ваш выбор: ");

            if (!int.TryParse(Console.ReadLine(), out int orderChoice) || orderChoice < 1 || orderChoice > 2)
            {
                Console.WriteLine("Неверный выбор.");
                return;
            }

            string[] sortCriteria = { "", "name", "employees", "created", "modified" };
            bool ascending = orderChoice == 1;

            var sortedDepartments = _departmentManager.SortDepartments(sortCriteria[sortChoice], ascending);

            Console.WriteLine($"Отсортированные подразделения ({sortCriteria[sortChoice]}, {(ascending ? "по возрастанию" : "по убыванию")}):");
            Console.WriteLine();

            foreach (var department in sortedDepartments)
            {
                Console.WriteLine(department.ToString());
                Console.WriteLine(new string('-', 80));
            }
        }

        /// <summary>
        /// Shows statistics
        /// </summary>
        private void ShowStatistics()
        {
            Console.WriteLine("=== СТАТИСТИКА ===");
            Console.WriteLine(_departmentManager.GetStatistics());
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
