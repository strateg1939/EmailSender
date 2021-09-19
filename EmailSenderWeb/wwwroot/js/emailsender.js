
function subs_send_data() {
    var all_subscriptions = $("#subscription_for_user .option-input");
    var send_data_actual = { to_subscribe: [], to_unsubscribe: [] };
    var send_data = { to_subscribe: {}, to_unsubscribe: {} };
    for (let i = 0; i < all_subscriptions.length; i++) {
        if (all_subscriptions[i].checked) {
            send_data_actual.to_subscribe.push(all_subscriptions[i].name);
            send_data.to_subscribe[all_subscriptions[i].name] = all_subscriptions[i].value;
        }
    }
    var all_unsubscriptions = $("#unsubscription_for_user .option-input");
    for (let i = 0; i < all_unsubscriptions.length; i++) {
        if (all_unsubscriptions[i].checked) {
            send_data_actual.to_unsubscribe.push(all_unsubscriptions[i].name);
            send_data.to_unsubscribe[all_unsubscriptions[i].name] = all_unsubscriptions[i].value;
        }
    }
    var to_subscribe_length = Object.keys(send_data.to_subscribe).length;
    var to_unsubscribe_length = Object.keys(send_data.to_unsubscribe).length;
    if (to_subscribe_length === 0 && to_unsubscribe_length === 0) {
        $("#response_text").css("display", "none");
        $("#response_text_hr").css("display", "none");
        return;
    }
    var ourRequest = new XMLHttpRequest();
    ourRequest.open('POST', '/api/Topics');


    ourRequest.onreadystatechange = function () {
        if (ourRequest.status >= 200 && ourRequest.status < 400 && ourRequest.readyState === 4) {
            if (ourRequest.responseText !== "success") {
                alert("Server error, try again later");
                console.log(ourRequest.responseText)
                return;
            }

            $("#response_text").css("display", "inline");
            $("#response_text_hr").css("display", "block");
            for (let topic_id in send_data.to_subscribe) {
                var topic = send_data.to_subscribe[topic_id];
                $("#" + topic).remove();
                $("#" + topic + "text").remove();
                $("#unsubscription_for_user").append("<p><input type='checkbox' id = \'" + topic + "\'  class = 'option-input' value =  \'" + topic + "\' name = \'" + topic_id + "\' ><span class = 'checkbox_text' id = \'" + topic + "text\' > " + topic + " </span></p>");

            }
            for (let topic_id in send_data.to_unsubscribe) {
                var topic = send_data.to_unsubscribe[topic_id];
                $("#" + topic).remove();
                $("#" + topic + "text").remove();
                $("#subscription_for_user").append("<p><input type='checkbox' id = \'" + topic + "\'  class = 'option-input' value = \'" + topic + "\'  name = \'" + topic_id + "\'><span class = 'checkbox_text' id = \'" + topic + "text\' > " + topic + " </span></p>");
            }
            if ($("#subscription_for_user .option-input").length > 0) {
                $("#another_text_sub").css("display", "inline");
            }
            else {
                $("#another_text_sub").css("display", "none");
                $("#hr").css("display", "none");
            }

            if ($("#unsubscription_for_user .option-input").length > 0) {
                $("#another_text_unsub").css("display", "inline");
            }
            else {
                $("#another_text_unsub").css("display", "none");
                $("#hr").css("display", "none");
            }
            if ($("#hr").css("display", "none") && $("#subscription_for_user .option-input").length > 0 && $("#unsubscription_for_user .option-input").length > 0) {
                $("#hr").css("display", "block");
            }
        }

    };

    ourRequest.onerror = function () {
        console.log("Connection error");
    };
    ourRequest.setRequestHeader("Content-Type", "application/json; charset=UTF-8");
    ourRequest.send(JSON.stringify(send_data_actual));

}

var url = new URL(window.location.href);
if (url.pathname.endsWith("Topics")) {
    var subs_submit = $("#subs_submit");
    if (subs_submit) {
        subs_submit.on("click", subs_send_data);
    }
    $('document').ready(
        function () {
            if ($("#subscription_for_user .option-input").length === 0) {
                $("#another_text_sub").css("display", "none");
                $("#hr").css("display", "none");
            }
            if ($("#unsubscription_for_user .option-input").length === 0) {
                $("#another_text_unsub").css("display", "none");
                $("#hr").css("display", "none");
            }
            if ($("#hr").css("display", "none") && $("#subscription_for_user .option-input").length > 0 && $("#unsubscription_for_user .option-input").length > 0) {
                $("#hr").css("display", "block");
            }
        }
    )
}



