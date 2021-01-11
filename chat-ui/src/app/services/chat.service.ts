import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Common } from './common/common';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  constructor(private http: HttpClient) { }
   public sendMessage(data: any)
  {
    const body = JSON.stringify(data);
    const reqHeader = new HttpHeaders({ 'Content-Type': 'application/json', 'No-Auth': 'True' });
    return this.http.post(Common.baseUrl + 'api/Chat/SendMessage', body, { headers: reqHeader });

  }
  public getMessage(data:any){
    const body = JSON.stringify(data);
    const reqHeader = new HttpHeaders({ 'Content-Type': 'application/json', 'No-Auth': 'True' });
    return this.http.post<any>(Common.baseUrl + 'api/Chat/getMessage', body, { headers: reqHeader });
    // const url = Common.baseUrl + 'api/Chat/getMessage?receiverId='+ receiverId;
    // return this.http.get<any>(url);//`${receiverId}`
  }
  public delete(id:number){
    const reqHeader = new HttpHeaders({ 'Content-Type': 'application/json', 'No-Auth': 'True' });
    return this.http.delete<boolean>(Common.baseUrl + 'api/Chat/delete?id=' + id, { headers: reqHeader });
  }
  public deleteOneSide(id:number){
    const reqHeader = new HttpHeaders({ 'Content-Type': 'application/json', 'No-Auth': 'True' });
    return this.http.put<boolean>(Common.baseUrl + 'api/Chat/DeleteOneSide?id=' + id, { headers: reqHeader });
  }
}
