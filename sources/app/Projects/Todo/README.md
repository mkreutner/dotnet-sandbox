# Projet Todo

## 1. Création et structure du Projet

Le Projet Todo aura la structure suivante :

```text
MyTodoApi/
├── MyTodoApi.csproj
├── Program.cs
├── Models/
│   └── TodoItem.cs
├── Data/
│   └── TodoDbContext.cs
└── Services/
    ├── ITodoService.cs
    └── TodoService.cs
```

On le crée avec la commande suivante :

```bash
dotnet new console
```

On ajoute ensuite les dépendances du Framework Core

```bash
# Ajout du support pour SQL Server
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

# Ajout des outils de conception (requis pour les migrations)
dotnet add package Microsoft.EntityFrameworkCore.Design

# Installation globale de l'outil CLI d'EF Core dans ton conteneur
dotnet tool install --global dotnet-ef
```

Créer et appliquer la première Migration

```bash
# Génération de la première Migration
dotnet ef migrations add InitialCreate

# Création de la base de données
dotnet ef database update
```

## 2. Développement

### 2.1. Appels curl pour tester

```bash
# 1. Ajouter une tâche urgente
curl -X POST http://localhost:5000/api/todo \
     -H "Content-Type: application/json" \
     -d '{"title":"Migrer le PC sur AwesomeWM","isCompleted":false}'

# 2. Récupérer la liste (Vérifie bien que l'ID généré est 1)
curl http://localhost:5000/api/todo

# 3. Marquer la tâche comme complétée !
curl -X PUT http://localhost:5000/api/todo/1/complete
```

Vous devez avoir quelque chose comme suit :

```bash
# Request
curl -X POST http://localhost:5000/api/todo \
     -H "Content-Type: application/json" \
     -d '{"title":"Migrer le PC sur AwesomeWM","isCompleted":false}'

# Resonse
{"id":1,"title":"Migrer le PC sur AwesomeWM","isCompleted":false}

# Request
curl http://localhost:5000/api/todo

# Response
[{"id":1,"title":"Migrer le PC sur AwesomeWM","isCompleted":false}]

# Request
curl -X PUT http://localhost:5000/api/todo/1/complete

# Response
{"message":"Todo 1 marked as completed."}

# Request
curl -X PUT http://localhost:5000/api/todo/1/complete

#Response ; key isCompleted has changed (false -> true)
[{"id":1,"title":"Migrer le PC sur AwesomeWM","isCompleted":true}]

```
