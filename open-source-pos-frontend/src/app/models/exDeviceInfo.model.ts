import { DeviceInfo } from 'ngx-device-detector';


export interface exDeviceInfo extends DeviceInfo {
    isMobile: boolean;
    isTablet: boolean;
    isDesktop: boolean;
}