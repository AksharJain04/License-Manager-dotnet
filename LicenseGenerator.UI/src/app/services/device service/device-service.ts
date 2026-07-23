import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { PagedResult } from '../../models/license-models/paged-result.dto';
import { InsertDeviceDto } from '../../models/device-models/insert-device.dto';
import { DeviceDto } from '../../models/device-models/device.dto';

@Injectable({
  providedIn: 'root'
})

export class DeviceService {

  private http = inject(HttpClient);
  
  private readonly apiUrl = 'http://localhost:5248/api/device';
  private readonly dlUrl = 'http://localhost:5248/api/devicelist';

  getDevices( page: number=1, pageSize: number=10 ): Observable<PagedResult<DeviceDto>> {
    return this.http.get<PagedResult<DeviceDto>>(
      `${this.dlUrl}?page=${page}&pageSize=${pageSize}`);
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
