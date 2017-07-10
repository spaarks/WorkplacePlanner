import { Injectable } from '@angular/core'

import { Message } from 'primeng/primeng'

export class MessageService {

    public msgs: Message[] = [];

    showSuccess(msg: string) {
        this.msgs = [];
        this.msgs.push({severity:'success', summary:'Success Message', detail:msg});
    }

    showInfo(msg: string) {
        this.msgs = [];
        this.msgs.push({severity:'info', summary:'Info Message', detail: msg});
    }

    showWarn(msg: string) {
        this.msgs = [];
        this.msgs.push({severity:'warn', summary:'Warn Message', detail: msg});
    }

    showError(msg: string) {
        this.msgs = [];
        this.msgs.push({severity:'error', summary:'Error Message', detail: msg});
    }

    clear() {
        this.msgs = [];
    }
}