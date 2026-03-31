import { Component } from '@angular/core';
import { TaskService } from '../../../core/services/task';
import { catchError, Observable, of, Subject, switchMap, startWith } from 'rxjs';
import { Task } from '../../../core/models/task.model';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators, FormsModule } from '@angular/forms';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.html',
  styleUrl: './task-list.scss',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule]
})
export class TaskList {

  private refresh$ = new Subject<void>();

  loading = true;
  error = false;
  editingTaskId: number | null = null;
  editTitle: string = '';

  form!: ReturnType<FormBuilder['group']>;

  tasks$: Observable<Task[]> = this.refresh$.pipe(
    startWith(void 0),
    switchMap(() => {
      this.loading = false;
      return this.taskService.getTasks();
    }),
    catchError(err => {
      console.error(err);
      this.error = true;
      return of([]);
    })
  );

  constructor(private taskService: TaskService, private fb: FormBuilder) {
    this.form = this.fb.nonNullable.group({
      title: ['', Validators.required],
      priority: ['medium']
    });
  }

  refresh() {
    this.refresh$.next();
  }

  add() {
    if (this.form.invalid) return;

    const value = this.form.value;

    this.taskService.addTask({
      title: value.title ?? '',
      priority: value.priority ?? 'medium'
    }).subscribe(() => {
      this.form.reset({ priority: 'medium' });
      this.refresh();
    });
  }

  delete(id: number) {
    this.taskService.deleteTask(id).subscribe(() => this.refresh());
  }

  toggle(task: Task) {
    this.taskService.updateTask({
      ...task,
      completed: !task.completed
    }).subscribe(() => this.refresh());
  }

  updateTitle(task: Task, event: FocusEvent) {
    const el = event.target as HTMLElement;
    const newTitle = el.innerText.trim();
    if (newTitle && newTitle !== task.title) {
      this.taskService.updateTask({ ...task, title: newTitle }).subscribe(() => this.refresh());
    } else {
      el.innerText = task.title;
    }
  }

  startEdit(task: Task) {
    this.editingTaskId = task.id;
    this.editTitle = task.title;
  }

  cancelEdit() {
    this.editingTaskId = null;
    this.editTitle = '';
  }

  saveEdit(task: Task) {
    if (!this.editTitle.trim()) return;

    this.taskService.updateTask({
      ...task,
      title: this.editTitle.trim()
    }).subscribe(() => {
      this.editingTaskId = null;
      this.editTitle = '';
      this.refresh();
    });
  }
}
