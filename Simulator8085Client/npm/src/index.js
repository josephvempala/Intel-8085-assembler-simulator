import 'ace-builds/src-noconflict/mode-assembly8085'
import * as ace from 'ace-builds';

window.InteropFunctions={
    initializeEditor : () => {
        ace.config.set('basePath', '/ace');
        const editor = ace.edit("editor");
        editor.session.setMode("ace/mode/assembly8085");
        editor.resize();
        editor.on('guttermousedown', function (e) {
        let target = e.domEvent.target;
        if (target.className.indexOf("ace_gutter-cell") == -1)
            return;
        if (!editor.isFocused())
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
}