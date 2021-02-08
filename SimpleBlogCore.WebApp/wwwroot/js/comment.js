$(function () {

    var prevCommentBlock = null;
    var commentBlock = $('.comment-respond > div');

    function init() {
        $('.comment-respond form').submit(function (e) {
            e.preventDefault();
            callAjax("/Comment/Add", {
                commentText: $(this).find("textarea").val().trim(),
                repliedToCommentId: null,
                postId: $(".entry__header").data("id")
            });
        });

        $(".reply").click(replyHandler);
        $(".cancel").click(cancelHandler);
        $(".edit").click(editHandler);
        $(".delete").click(deleteHandler);
    }

    function setInitialStateForPrevCommentBlock() {
        if (prevCommentBlock !== null) {
            $(prevCommentBlock)
                .siblings('.comment-controls')
                .find("a.comment-action")
                .each(function () {
                    $(this).toggle();
                });
            $(prevCommentBlock).remove();
        }
    }

    function replyHandler() {
        setInitialStateForPrevCommentBlock();

        prevCommentBlock = $(commentBlock).clone();
        var commentControlsBlock = $(this).parent();
        commentControlsBlock.after(prevCommentBlock);

        $(this).parent().find("a.comment-action").each(function () {
            $(this).toggle();
        });

        prevCommentBlock.find("form").submit((e) => {
            e.preventDefault();
            callAjax("/Comment/Add", {
                commentText: prevCommentBlock.find("textarea").val().trim(),
                repliedToCommentId: $(this).parent().parent().data("id") || null,
                postId: $(".entry__header").data("id")
            });
        });
    };

    function cancelHandler() {
        $(prevCommentBlock).remove();

        $(this).parent().find("a.comment-action").each(function () {
            $(this).toggle();
        });
    }

    function editHandler() {
        setInitialStateForPrevCommentBlock();

        prevCommentBlock = $(commentBlock).clone();
        // set value to textarea from comment
        var commText = $(this).parent().parent().find(".comment__text p").html().trim();
        prevCommentBlock.find("textarea").val(commText);
        $(this).parent().after(prevCommentBlock);

        prevCommentBlock.find("input").val("Save");;

        $(this).parent().find("a.comment-action").each(function () {
            $(this).toggle();
        });

        prevCommentBlock.find("form").submit((e) => {
            e.preventDefault();
            callAjax("/Comment/Edit", {
                commentId: $(this).parent().parent().data("id"),
                commentText: prevCommentBlock.find("textarea").val().trim()
            });
        });
    }

    function deleteHandler() {
        if (confirm("Do you really want to delete the comment?")) {
            callAjax("/Comment/Delete", {
                commentId: $(this).parent().parent().data("id")
            });
        }
    }

    function callAjax(url, data) {
        $.ajax({
            url,
            method: "POST",
            data,
            success: function (partialView) {
                $("#comments").html(partialView);
                commentBlock = $('.comment-respond > div');
                init();
            }
        });
    }

    init();
});
