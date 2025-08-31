# Project Progress API Documentation

## Endpoint: GET /api/project/{projectId}/progress

This endpoint should return the project progress data for the specified project.

### Response Structure

```json
{
  "projectId": "123",
  "overallProgress": 67,
  "tasksCompleted": 18,
  "totalTasks": 27,
  "daysRemaining": 35,
  "tasks": [
    {
      "id": "task-1",
      "title": "Foundation Work",
      "description": "Complete foundation excavation and concrete pouring",
      "status": "completed",
      "dueDate": "2025-08-20",
      "completedDate": "2025-08-18",
      "assignee": "John Smith",
      "priority": "high"
    },
    {
      "id": "task-2",
      "title": "Steel Framework",
      "description": "Install steel framework for main structure",
      "status": "in-progress",
      "dueDate": "2025-09-10",
      "assignee": "Mike Johnson",
      "priority": "high"
    }
  ],
  "recentActivities": [
    {
      "id": "activity-1",
      "description": "Foundation work completed ahead of schedule",
      "date": "2025-08-26",
      "type": "success"
    },
    {
      "id": "activity-2",
      "description": "Steel framework delivery scheduled",
      "date": "2025-08-23",
      "type": "info"
    }
  ]
}
```

### Field Descriptions

- `projectId`: The unique identifier of the project
- `overallProgress`: Overall project completion percentage (0-100)
- `tasksCompleted`: Number of completed tasks
- `totalTasks`: Total number of tasks in the project
- `daysRemaining`: Number of days remaining until project deadline
- `tasks`: Array of project tasks with their details
- `recentActivities`: Array of recent project activities/updates

### Task Status Values

- `completed`: Task is finished
- `in-progress`: Task is currently being worked on
- `pending`: Task is waiting to be started
- `overdue`: Task is past its due date

### Priority Values

- `high`: High priority task
- `medium`: Medium priority task
- `low`: Low priority task

### Activity Types

- `success`: Positive updates (completions, achievements)
- `info`: Informational updates (schedules, deliveries)
- `warning`: Issues that need attention (overdue items, blockers)

## Additional Endpoints

### Update Task Status: PUT /api/project/{projectId}/tasks/{taskId}/status

```json
{
  "status": "completed"
}
```

### Add New Task: POST /api/project/{projectId}/tasks

```json
{
  "title": "New Task",
  "description": "Task description",
  "status": "pending",
  "dueDate": "2025-09-15",
  "assignee": "John Doe",
  "priority": "medium"
}
```
