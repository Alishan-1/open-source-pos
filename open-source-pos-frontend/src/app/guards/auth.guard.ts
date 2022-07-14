import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, ActivatedRoute, RouterState } from '@angular/router';
import { AuthService } from '../views/login/auth.service';

@Injectable()
export class AuthGuard implements CanActivate {
  token: string | undefined;
  constructor(private authService: AuthService, private router: Router, private route: ActivatedRoute) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    debugger;

    this.token = route.queryParams["Q"];   

    if (this.token) {
      //token provided so get current user from backend
      // this.showLoader = true;
      this.authService.GetCurrentUser({
        Token: this.token,
      }).subscribe(
        usr => {
          debugger;
          // this.showLoader = false;
          // console.log(sr.Data);
          let user: any = usr;
          user.Token = this.token
          localStorage.setItem('currentUser', JSON.stringify(user));
          // console.log(usr);
        });


      return true;
    }
    let currentUser = JSON.parse(localStorage.getItem('currentUser') || '{}');
    if (currentUser && currentUser.Token) {
        // logged in so return true
        //token provided so get current user from backend
      // this.showLoader = true;
      this.authService.GetCurrentUser({
        Token: currentUser.Token,
      }).subscribe({
        next: usr => {
          debugger;
          // this.showLoader = false;
          // console.log(sr.Data);
          // let user: any = usr;
          // user.Token = this.token
          // localStorage.setItem('currentUser', JSON.stringify(user));
          // console.log(usr);

          return true;
        },
        error: e=>{
          localStorage.removeItem('currentUser');
          this.router.navigate(['login'], { queryParams: { returnUrl: state.url } });
          return false;      
        }
      });
      return true;
    }
    
    // not logged in so redirect to login page with the return url
    this.router.navigate(['login'], { queryParams: { returnUrl: state.url } });
    return false;
    }
}
