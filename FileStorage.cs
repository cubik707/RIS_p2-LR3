using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace RIS_p2_LR3
{
    public class FileStorage
    {
        private readonly string _filePath;

        public FileStorage(string filePath = "departments.json")
        {
            _filePath = filePath;
        }

        public bool SaveDepartments(List<Department> departments)
        {
            try
            {
                var jsonDepartments = departments.ConvertAll(d => DepartmentJson.FromDepartment(d));
                var serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(jsonDepartments);
                File.WriteAllText(_filePath, json, Encoding.UTF8);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении данных: {ex.Message}");
                return false;
            }
        }

        public List<Department> LoadDepartments()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new List<Department>();
                }

                string json = File.ReadAllText(_filePath, Encoding.UTF8);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<Department>();
                }

                var serializer = new JavaScriptSerializer();
                var jsonDepartments = serializer.Deserialize<List<DepartmentJson>>(json);
                
                if (jsonDepartments != null)
                {
                    return jsonDepartments.ConvertAll(jd => jd.ToDepartment());
                }
                
                return new List<Department>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
                return new List<Department>();
            }
        }

        public bool FileExists()
        {
            return File.Exists(_filePath);
        }

        public bool CreateBackup()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return false;
                }

                string backupPath = $"{_filePath}.backup_{DateTime.Now:yyyyMMdd_HHmmss}";
                File.Copy(_filePath, backupPath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании резервной копии: {ex.Message}");
                return false;
            }
        }
    }
}
