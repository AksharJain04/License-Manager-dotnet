import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { InsertDeviceDto } from '../../models/device-models/insert-device.dto';
import { DeviceDto } from '../../models/device-models/device.dto';

@Injectable({
  providedIn: 'root'
})
export class DeviceService {

  private http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5248/api/device';
  private readonly dlUrl = 'http://localhost:5248/api/devicelist';

  getAllDevices(): Observable<DeviceDto[]> {
    return this.http.get<DeviceDto[]>(this.dlUrl);
  }

  getDevice(deviceId: string): Observable<DeviceDto> {
    return this.http.get<DeviceDto>(`${this.apiUrl}/${deviceId}`);
  }

  createDevice(device: InsertDeviceDto): Observable<DeviceDto> {
    return this.http.post<DeviceDto>(this.apiUrl, device);
  }

  updateDeviceStatus(deviceId: string, status: string): Observable<DeviceDto> {
            return this.http.patch<DeviceDto>(
              `${this.apiUrl}/${deviceId}`,
              {deviceStatus: status});
  }

  deleteDevice(deviceId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${deviceId}`);
  }

}
