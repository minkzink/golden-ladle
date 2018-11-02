$(document).ready(function (e) {

    var selectClass = "";
    var selectId = "";
    var parentClass = "";
    var entryId = 0;
    var entryValue = 0;
    var eventId = 0;
    var userId = "";
    var voteId = 0;
    var tipobj;

    tippy('[title]');

    $(".vote1, .vote2, .vote3").on('click', function () {
        selectId = $(this).attr("id");
        let startIndex = $(this).attr("class").toString().indexOf("vote");
        selectClass = $(this).attr("class").substring(startIndex, startIndex + 5);
        parentClass = $(this).parent().attr("class").toString();
        entryId = $(this).attr("id").split('_')[1];
        entryValue = $(this).attr("value");
        eventId = $("#eventId").attr("value");
        userId = $("#loggedInUser").attr("value");
        voteId = null;
        voteId = $(this).attr("data-voteid");

        $('body').toggleClass('loaded').attr("noscroll");
        if ($(this).hasClass("disabled") && voteId !== undefined) {
            deleteToolTip(parentClass, selectId, selectClass);
            deleteVote(voteId, parentClass, selectClass, selectId);
        }
        else if (!$(this).hasClass("disabled")) {
            addTooltip(parentClass, selectId, selectClass);
            postVote(entryValue, entryId, eventId, userId, parentClass, selectClass, selectId);
        }
        else {
            $('body').toggleClass('loaded').attr("noscroll");
        }
    });

    function getVoteId(userId, entryId, parentClass, selectClass) {
        $.ajax({
            type: "GET",
            url: `/api/Votes/get?userId=${userId}&entryId=${entryId}`,
            dataType: 'json',
            success: function (response) {
                voteId = response.id;
                console.log(`GetVoteId ${voteId} Successful!`);
                $(`#${selectId}`).attr('data-voteId', `${voteId}`);
            },
            error: function () {
                alert('Error during AJAX.');
                console.log(parentClass, selectClass);
            }
        });
    }

    function postVote(entryValue, entryId, eventId, userId, parentClass, selectClass, selectId) {
        $.ajax({
            type: "POST",
            url: "/api/Votes",
            data: {
                "id": 0,
                "value": entryValue,
                "entryId": entryId,
                "eventId": eventId,
                "userId": `${userId}`
            },
            dataType: 'json',
            success: function () {
                getVoteId(userId, entryId, parentClass, selectClass);
                addTooltip(voteId, parentClass, selectId, selectClass, entryValue);
                $('body').toggleClass('loaded').removeAttr("noscroll");
            },
            error: function () {
                console.log('Error during AJAX.');
                $('body').toggleClass('loaded').removeAttr("noscroll");
            }
        });
    }

    function deleteVote(voteId, parentClass, selectClass, selectId) {
        $.ajax({
            type: "DELETE",
            url: `/api/Votes/${voteId}`,
            dataType: 'json',
            success: function () {
                deleteToolTip(parentClass, selectId, selectClass);
                $('body').toggleClass('loaded').removeAttr("noscroll");
            },
            error: function () {
                console.log('AJAX error in request.');
                addTooltip(parentClass, selectId, selectClass);
                $('body').toggleClass('loaded').removeAttr("noscroll");
            }
        });
    }
    function deleteToolTip(parentClass, selectId, selectClass) {
        $(`#${selectId}`).addClass("btn-outline-success");
        $(`#${selectId}`).removeClass("btn-success");
        $(`#${selectId}`).parent().removeAttr('title data-tippy data-original-title');
        $(`.${parentClass}`).removeAttr('title data-tippy data-original-title');
        $(`.${selectClass}`).removeClass("disabled");
        tipobj = tippy(`.${parentClass}`);
        tipobj.destroyAll();
    }

    function addTooltip(voteId, parentClass, selectId, selectClass, entryValue) {
        $(`.${parentClass}`).attr('title', `You Already voted for #${entryValue}.`);
        $(`#${selectId}`).parent().attr('title', `<strong>Voted #${entryValue}</strong> Click to undo.`);
        tipobj = tippy(`.${parentClass}`);
        $(`.${selectClass}`).toggleClass("disabled");
        $(`#${selectId}`).toggleClass("btn-outline-success");
        $(`#${selectId}`).toggleClass("btn-success");
    }
});