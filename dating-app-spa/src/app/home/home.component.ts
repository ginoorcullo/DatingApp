import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode: boolean = false;


  constructor() { }

  ngOnInit(): void {
  }

  registerToggle() {
    this.registerMode = true;
  }

  // getValues(){
  //   this.http.get('http://localhost:5000/api/values/get-values').subscribe(response => {
  //     this.values = response;
  //     console.log(this.values);
  //   }, error => {
  //     console.log(error);
  //   });
  // }

}
