import alertify from 'alertifyjs';

import 'alertifyjs/build/css/alertify.min.css';
import 'alertifyjs/build/css/themes/default.min.css'
import '../assets/css/alertify.patch.css'

const NOTIFY_WAIT = 2;

alertify.set('notifier', 'delay', NOTIFY_WAIT);
alertify.set('notifier', 'position', 'top-center');

const EMOJI_HAPPY = ['😀', '😁', '😃', '😄', '😆', '😊', '😉', '😋', '😚', '🤗', '🤩', '🙂', '😍', '🥰', '😘', '🥳'];
const EMOJI_GOOD = ['🙂', '🙂', '😌', '🤠', '🤪', '😝', '😜', '😛', '🤨'];
const EMOJI_BAD = ['😶', '😥', '😫', '😓', '😣', '😖', '😩', '😰', '😱', '😵', '😵‍💫', '🥴', '🤕', '🤒'];

function getRandom(arr) {
    return arr[Math.min(Math.floor(Math.random() * arr.length), arr.length)];
}

class Notifier {
    success(msg, dismiss = false, wait = NOTIFY_WAIT) {
        const a = alertify.notify(msg + getRandom(EMOJI_HAPPY), 'c-success', wait);
        if (dismiss) {
            a.dismissOthers();
        }
    }

    info(msg, dismiss = false, wait = NOTIFY_WAIT) {
        const a = alertify.notify(msg + getRandom(EMOJI_GOOD), 'c-info', wait);
        if (dismiss) {
            a.dismissOthers();
        }
    }

    warning(msg, dismiss = false, wait = NOTIFY_WAIT) {
        const a = alertify.notify(msg, 'c-warning', wait);
        if (dismiss) {
            a.dismissOthers();
        }
    }

    error(msg, dismiss = false, wait = NOTIFY_WAIT) {
        const a = alertify.notify(msg + getRandom(EMOJI_BAD), 'c-error', wait);
        if (dismiss) {
            a.dismissOthers();
        }
    }

    auto(meta, successMsg = null, errorMsg = null, dismiss = false, wait = NOTIFY_WAIT) {
        if (meta.status == 0) {
            this.success(successMsg ?? meta.message, dismiss, wait);
        } else {
            this.error(errorMsg ?? meta.message, dismiss, wait);
        }
    }

    notify(type, msg, dismiss = false, wait = NOTIFY_WAIT) {
        const a = alertify.notify(msg, type, wait);
        if (dismiss) {
            a.dismissOthers();
        }
    }

    notifyWithCountDown(type, msg, duration, dismiss = false) {
        if (duration <= 0) {
            this.notify(type, msg, dismiss, NOTIFY_WAIT);
        }
        var a = alertify.notify(msg + ' (' + duration + 's)', type, duration + 1, function () { clearInterval(interval); });
        var interval = setInterval(function () {
            a.setContent(msg + ' (' + (--duration) + 's)');
        }, 1000);
    }
}

var notifier = new Notifier();

export default notifier;