using System;
using System.Collections.Generic;
using System.Linq;

namespace RIS_p2_LR3
{
    /// <summary>
    /// Manages department operations including CRUD, search and sort
    /// </summary>
    public class DepartmentManager
    {
        private List<Department> _departments;
        private readonly FileStorage _fileStorage;
        private int _nextId;

        public DepartmentManager()
        {
            _fileStorage = new FileStorage();
            _departments = _fileStorage.LoadDepartments();
            _nextId = _departments.Count > 0 ? _departments.Max(d => d.Id) + 1 : 1;
        }

        /// <summary>
        /// Gets all departments
        /// </summary>
        /// <returns>List of all departments</returns>
        public List<Department> GetAllDepartments()
        {
            return new List<Department>(_departments);
        }

        /// <summary>
        /// Adds a new department
        /// </summary>
        /// <param name="name">Department name</param>
        /// <param name="description">Department description</param>
        /// <param name="employeeCount">Number of employees</param>
        /// <returns>True if added successfully, false otherwise</returns>
        public bool AddDepartment(string name, string description, int employeeCount)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Название подразделения не может быть пустым.");
                return false;
            }

            if (employeeCount < 0)
            {
                Console.WriteLine("Количество сотрудников не может быть отрицательным.");
                return false;
            }

            // Check if department with same name already exists
            if (_departments.Any(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Подразделение с таким названием уже существует.");
                return false;
            }

            var department = new Department(_nextId++, name.Trim(), description?.Trim() ?? "", employeeCount);
            _departments.Add(department);

            if (SaveData())
            {
                Console.WriteLine($"Подразделение '{name}' успешно добавлено.");
                return true;
            }

            _departments.Remove(department);
            _nextId--;
            return false;
        }

        /// <summary>
        /// Updates an existing department
        /// </summary>
        /// <param name="id">Department ID</param>
        /// <param name="name">New name</param>
        /// <param name="description">New description</param>
        /// <param name="employeeCount">New employee count</param>
        /// <returns>True if updated successfully, false otherwise</returns>
        public bool UpdateDepartment(int id, string name, string description, int employeeCount)
        {
            var department = _departments.FirstOrDefault(d => d.Id == id);
            if (department == null)
            {
                Console.WriteLine("Подразделение с указанным ID не найдено.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Название подразделения не может быть пустым.");
                return false;
            }

            if (employeeCount < 0)
            {
                Console.WriteLine("Количество сотрудников не может быть отрицательным.");
                return false;
            }

            // Check if another department with same name exists
            if (_departments.Any(d => d.Id != id && d.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Подразделение с таким названием уже существует.");
                return false;
            }

            var oldName = department.Name;
            department.Name = name.Trim();
            department.Description = description?.Trim() ?? "";
            department.EmployeeCount = employeeCount;
            department.LastModifiedDate = DateTime.Now;

            if (SaveData())
            {
                Console.WriteLine($"Подразделение '{oldName}' успешно обновлено.");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Deletes a department by ID
        /// </summary>
        /// <param name="id">Department ID</param>
        /// <returns>True if deleted successfully, false otherwise</returns>
        public bool DeleteDepartment(int id)
        {
            var department = _departments.FirstOrDefault(d => d.Id == id);
            if (department == null)
            {
                Console.WriteLine("Подразделение с указанным ID не найдено.");
                return false;
            }

            var departmentName = department.Name;
            _departments.Remove(department);

            if (SaveData())
            {
                Console.WriteLine($"Подразделение '{departmentName}' успешно удалено.");
                return true;
            }

            _departments.Add(department);
            return false;
        }

        /// <summary>
        /// Searches departments by name or description
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of matching departments</returns>
        public List<Department> SearchDepartments(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return GetAllDepartments();
            }

            var term = searchTerm.ToLower();
            return _departments.Where(d => 
                d.Name.ToLower().Contains(term) || 
                d.Description.ToLower().Contains(term))
                .ToList();
        }

        /// <summary>
        /// Sorts departments by specified criteria
        /// </summary>
        /// <param name="sortBy">Sort criteria: name, employees, created, modified</param>
        /// <param name="ascending">True for ascending order, false for descending</param>
        /// <returns>Sorted list of departments</returns>
        public List<Department> SortDepartments(string sortBy, bool ascending = true)
        {
            IOrderedEnumerable<Department> sorted;

            switch (sortBy.ToLower())
            {
                case "name":
                    sorted = ascending ? _departments.OrderBy(d => d.Name) : _departments.OrderByDescending(d => d.Name);
                    break;
                case "employees":
                    sorted = ascending ? _departments.OrderBy(d => d.EmployeeCount) : _departments.OrderByDescending(d => d.EmployeeCount);
                    break;
                case "created":
                    sorted = ascending ? _departments.OrderBy(d => d.CreatedDate) : _departments.OrderByDescending(d => d.CreatedDate);
                    break;
                case "modified":
                    sorted = ascending ? _departments.OrderBy(d => d.LastModifiedDate) : _departments.OrderByDescending(d => d.LastModifiedDate);
                    break;
                default:
                    return GetAllDepartments();
            }

            return sorted.ToList();
        }

        /// <summary>
        /// Gets department statistics
        /// </summary>
        /// <returns>Statistics information</returns>
        public string GetStatistics()
        {
            if (!_departments.Any())
            {
                return "Нет данных о подразделениях.";
            }

            var totalDepartments = _departments.Count;
            var totalEmployees = _departments.Sum(d => d.EmployeeCount);
            var avgEmployees = totalEmployees / (double)totalDepartments;
            var maxEmployees = _departments.Max(d => d.EmployeeCount);
            var minEmployees = _departments.Min(d => d.EmployeeCount);

            return $"Общее количество подразделений: {totalDepartments}\n" +
                   $"Общее количество сотрудников: {totalEmployees}\n" +
                   $"Среднее количество сотрудников: {avgEmployees:F1}\n" +
                   $"Максимальное количество сотрудников: {maxEmployees}\n" +
                   $"Минимальное количество сотрудников: {minEmployees}";
        }

        /// <summary>
        /// Saves data to file
        /// </summary>
        /// <returns>True if saved successfully, false otherwise</returns>
        private bool SaveData()
        {
            return _fileStorage.SaveDepartments(_departments);
        }

        /// <summary>
        /// Creates backup of current data
        /// </summary>
        /// <returns>True if backup created successfully, false otherwise</returns>
        public bool CreateBackup()
        {
            return _fileStorage.CreateBackup();
        }
    }
}
