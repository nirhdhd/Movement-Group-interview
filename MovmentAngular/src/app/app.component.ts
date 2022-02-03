import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';

export interface Post {
  makor: string;
  title: number;
  date: Date;
  desc: string;
  link?: string;
}

let ELEMENT_DATA: Post[] = [];

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  displayedColumns: string[] = ['makor', 'title', 'date', 'desc'];
  //dataSource = ELEMENT_DATA;

  dataSource = new MatTableDataSource<Post>();

  ngOnInit() {
    this.dataSource.data = ELEMENT_DATA;
  }

  getPosts = async (e) => {
    var myHeaders = new Headers();
    myHeaders.append('Content-Type', 'application/json');

    var raw = JSON.stringify(e.value);

    var requestOptions = {
      method: 'POST',
      headers: myHeaders,
      body: raw,
    };

    fetch('https://localhost:7053/api/MM/GetRssPosts', requestOptions)
      .then((response) => response.json())
      .then((result) => {
        console.log(result);
        ELEMENT_DATA = [];
        result.map((p: Post) => {
          ELEMENT_DATA.push(p);
        });
        this.dataSource.data = ELEMENT_DATA;
      })

      .catch((error) => console.log('error', error));
  };

  title = 'movmentTest';
}
