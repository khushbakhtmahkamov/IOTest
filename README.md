1. Склонируйте репозиторий.
2. Перейдите в корень проекта и выполните эти команди.
	Миграция базы данных
		dotnet ef migrations add InitialCreate
		dotnet ef database update

	Запуск проекта
		dotnet run