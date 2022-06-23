export class ServiceResponse {
    constructor() {

    }
    public Title: string | undefined;
    public Message: string | undefined;
    public Flag: boolean | undefined;
    public Data: any;
    public LookUp: any;
    public StatusCode: any;
    public IsValid: boolean | undefined;
    public Errors: Array<any> | undefined;
}