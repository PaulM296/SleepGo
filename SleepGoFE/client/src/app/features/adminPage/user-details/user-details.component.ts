import { AfterViewInit, ChangeDetectorRef, Component, inject, OnInit, ViewChild } from '@angular/core';
import { ResponseUserModel } from '../../../shared/models/responseUserModel';
import { UserService } from '../../../core/services/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { CommonModule } from '@angular/common';
import { MatCard } from '@angular/material/card';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { MatPaginator } from '@angular/material/paginator';

@Component({
  selector: 'app-user-details',
  standalone: true,
  imports: [
    MatTableModule,
    MatButtonModule,
    MatToolbarModule,
    CommonModule,
    MatButtonModule,
    MatCard,
    MatPaginator
  ],
  templateUrl: './user-details.component.html',
  styleUrl: './user-details.component.scss'
})
export class UserDetailsComponent  implements OnInit, AfterViewInit {
  users: ResponseUserModel[] = [];
  dataSource = new MatTableDataSource<ResponseUserModel>();
  displayedColumns: string[] = ['username', 'email', 'firstName', 'lastName', 'actions'];

  private snackbarService = inject(SnackbarService);
  private cdr = inject(ChangeDetectorRef);
  private userService = inject(UserService);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngOnInit(): void {
    this.loadUsers();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.cdr.detectChanges();
  }

  loadUsers(): void {
    this.userService.getPaginatedUsers(1, 10).subscribe(response => {
      this.users = response.items;
      this.dataSource.data = response.items;
    });
  }

  toggleBlock(user: ResponseUserModel): void {
    if(user.isBlocked) {
      this.userService.unblockUser(user.id).subscribe({
        next: () => {
          user.isBlocked = false;
          this.snackbarService.success(`${user.userName} has been unblocked`);
        }, 
        error: () => {
          this.snackbarService.error(`Failed to unblock ${user.userName}. Please try again.`);
        }
      });
    } else {
      this.userService.blockUser(user.id).subscribe({
        next: () => {
          user.isBlocked = true;
          this.snackbarService.success(`${user.userName} has been blocked`);
        }, 
        error: () => {
          this.snackbarService.error(`Failed to unblock ${user.userName}. Please try agaian.`);
        }
      });
    }
  }

  previousPage(): void {
    if(this.paginator && this.paginator.hasPreviousPage()) {
      this.paginator.previousPage();
    }
  }

  nextPage(): void {
    if(this.paginator && this.paginator.hasNextPage()) {
      this.paginator.nextPage();
    }
  }

}
