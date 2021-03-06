import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Common } from '../common/common';
import { catchError, map } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { IpService } from '../ip.service';

// @Injectable({
//   providedIn: 'root'
// })
@Injectable()
export class AuthService {

  constructor(private http: HttpClient, private ipService: IpService) { }
  public authenticate(loginData: any)
  {
    const reqHeader = new HttpHeaders({'No-Auth': 'True'});
    return this.http.post<any>(Common.baseUrl + 'api/Users/login', loginData, {headers: reqHeader} )
    .pipe(map(user => {
      console.log(user);
      if(user.data != null){
      this.localStorageSet(user);
      this.loginStatus(user.data.userId, true);
      }
    }));
  }
  public signup(data: any)
  {
    const body = JSON.stringify(data);
    const reqHeader = new HttpHeaders({ 'Content-Type': 'application/json', 'No-Auth': 'True' });
    return this.http.post(Common.baseUrl + 'api/users', body, { headers: reqHeader });
  }

  getUsers(id:number){
    const reqHeader = new HttpHeaders({ 'No-Auth': 'True' });
    return this.http.get<any>(Common.baseUrl + 'api/Users/?UserId=' + id + '&Email=null&FirstName=null&LastName=null', { headers: reqHeader });
  }
  localStorageSet(user: any){
    localStorage.setItem('id', user.data.userId);
    localStorage.setItem('email', user.data.email);
    localStorage.setItem('firstName', user.data.firstName);
    localStorage.setItem('lastName', user.data.lastName);
  }
  loginStatus(id: number, isLogin: boolean){
    this.ipService.getIpCliente().subscribe((res:any)=>{  
      const data = {
        IpAddress: res.ip == undefined ? '0:0:0:0' : res.ip,
        UserId:id,
        IsLogin: isLogin
      }

      const body = JSON.stringify(data);
      console.log(body);
      const reqHeader = new HttpHeaders({'Content-Type': 'application/json','No-Auth': 'True'});
      const url = Common.baseUrl +'api/LoginStatus/addLoginStatus';
      return this.http.post<any>(url, body, { headers: reqHeader }).subscribe(p =>{
        console.log(p);
      });
    });  
    
  }
}
