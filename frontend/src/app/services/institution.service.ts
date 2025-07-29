import { Injectable } from '@angular/core';

export interface InstitutionConfig {
  id: number;
  name: string;
  logo: string;
  themeColor: string;
}

@Injectable({
  providedIn: 'root'
})
export class InstitutionService {
  private config: InstitutionConfig = {
    id: 0,
    name: '',
    logo: '',
    themeColor: ''
  };

  private readonly institutionMap: { [key: string]: InstitutionConfig } = {
    'beit-shemesh': {
      id: 1,
      name: 'גמ"ח בית שמש',
      logo: 'logo-beit-shemesh.png',
      themeColor: '#f7941d'
    },
    'bnei-brak': {
      id: 2,
      name: 'גמ"ח בני ברק',
      logo: 'logo-bnei-brak.png',
      themeColor: '#007bff'
    },
    'localhost': {
      id: 1,
      name: 'הגמ"ח המרכזי',
      logo: '',
      themeColor: '#222'
    }
  };

  constructor() {
    const hostname = window.location.hostname;
    const subdomain = hostname.includes('localhost') ? 'localhost' : hostname.split('.')[0];
    this.config = this.institutionMap[subdomain] ?? {
      id: 0,
      name: 'מוסד לא ידוע',
      logo: 'default-logo.png',
      themeColor: '#ccc'
    };
  }

  getInstitution(): InstitutionConfig {
    return this.config;
  }

  getInstitutionId(): number {
    return this.config.id;
  }
}
