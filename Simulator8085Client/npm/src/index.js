import 'ace-builds/src-noconflict/mode-assembly8085'
import * as ace from 'ace-builds';

window.editor = null;
window.InteropFunctions={
    initializeEditor : () => {
        ace.config.set('basePath', '/ace');
        window.editor = ace.edit("editor");
        window.editor.session.setMode("ace/mode/assembly8085");
        window.editor.resize();
        window.editor.on('guttermousedown', function (e) {
        let target = e.domEvent.target;
        if (target.className.indexOf("ace_gutter-cell") == -1)
            return;
        if (!window.editor.isFocused())
            return;
        if (e.clientX > 25 + target.getBoundingClientRect().left)
            return;
        let row = e.getDocumentPosition().row;
        let breakpoints = e.editor.session.getBreakpoints(row, 0);
        if (typeof breakpoints[row] === typeof undefined)
            e.editor.session.setBreakpoint(row);

        else
            e.editor.session.clearBreakpoint(row);
        e.stop();
        });
    },
    getCode: () => {
        return editor.getValue();
    },
}