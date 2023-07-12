import alertify from 'alertifyjs';

import 'alertifyjs/build/css/alertify.min.css';
import 'alertifyjs/build/css/themes/default.min.css'
import '../assets/css/alertify.patch.css'

const NOTIFY_WAIT = 1.5;

alertify.set('notifier', 'delay', NOTIFY_WAIT);
alertify.set('notifier', 'position', 'top-center');

const EMOJI = ["😀😁😃😄😆😊😉😋😚🤗🤩🙂"];

class Notifier {
    success(msg, dismiss = false, wait = NOTIFY_WAIT) {
        var a = alertify.notify(msg, "c-success", wait);
        if (dismiss) {
            a.dismissOthers();
        }
    }

    info(msg, dismiss = false, wait = NOTIFY_WAIT) {
        var a = alertify.notify(msg, "c-info", wait);
        if (dismiss) {
            a.dismissOthers();
        }
    }

    warning(msg, dismiss = false, wait = NOTIFY_WAIT) {
        var a = alertify.notify(msg, "c-warning", wait);
        if (dismiss) {
            a.dismissOthers();
        }
    }

    error(msg, dismiss = false, wait = NOTIFY_WAIT) {
        var a = alertify.notify(msg, "c-error", wait);
        if (dismiss) {
            a.dismissOthers();
        }
    }

    auto(meta, dismiss = false, wait = NOTIFY_WAIT) {
        if (meta.status == 0) {
            this.success(meta.message, dismiss, wait);
        } else {
            this.error(meta.message, dismiss, wait);
        }
    }
}

var notifier = new Notifier();

export default notifier;
