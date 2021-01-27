
function subs_send_data() {
	var all_subscriptions = document.getElementById("subscription_for_user").querySelectorAll(".option-input");
	var send_data_actual = { to_subscribe: [], to_unsubscribe: [] };
	var send_data = { to_subscribe: {}, to_unsubscribe: {} };
	for (let i = 0; i < all_subscriptions.length; i++) {
		if (all_subscriptions[i].checked) {
			send_data_actual.to_subscribe.push(all_subscriptions[i].name);
			send_data.to_subscribe[all_subscriptions[i].name] = all_subscriptions[i].value;
		}
	}
	var all_unsubscriptions = document.getElementById("unsubscription_for_user").querySelectorAll(".option-input");
	for (let i = 0; i < all_unsubscriptions.length; i++) {
		if (all_unsubscriptions[i].checked) {
			send_data_actual.to_unsubscribe.push(all_unsubscriptions[i].name);
			send_data.to_unsubscribe[all_unsubscriptions[i].name] = all_unsubscriptions[i].value;
		}
	}
	var to_subscribe_length = Object.keys(send_data.to_subscribe).length;
	var to_unsubscribe_length = Object.keys(send_data.to_unsubscribe).length;
	console.log(send_data)
	if (to_subscribe_length === 0 && to_unsubscribe_length === 0) {
		document.getElementById("response_text").style.display = "none";
		document.getElementById("response_text_hr").style.display = "none";
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

			document.getElementById("response_text").style.display = "inline";
			document.getElementById("response_text_hr").style.display = "block";
			for (let topic_id in send_data.to_subscribe) {
				var topic = send_data.to_subscribe[topic_id];
				document.getElementById(topic).remove();
				document.getElementById(topic + "+.text").remove();
				document.getElementById("unsubscription_for_user").innerHTML += "<p><input type='checkbox' id = \'" + topic + "\'  class = 'option-input' value =  \'" + topic + "\' name = \'" + topic_id + "\' ><span class = 'checkbox_text' id = \'" + topic + "+.text\' > " + topic + " </span></p>";

			}
			for (let topic_id in send_data.to_unsubscribe) {
				var topic = send_data.to_unsubscribe[topic_id];
				document.getElementById(topic).remove();
				document.getElementById(topic + "+.text").remove();
				document.getElementById("subscription_for_user").innerHTML += "<p><input type='checkbox' id = \'" + topic + "\'  class = 'option-input' value = \'" + topic + "\'  name = \'" + topic_id + "\'><span class = 'checkbox_text' id = \'" + topic + "+.text\' > " + topic + " </span></p>";
			}
			if (document.getElementById("another_text_sub").style.display === "none" && document.getElementById("subscription_for_user").querySelectorAll(".option-input").length > 0) {
				document.getElementById("another_text_sub").style.display = "inline";
			}
			else if ((document.getElementById("another_text_sub").style.display === "inline" || document.getElementById("another_text_sub").style.display === "") && document.getElementById("subscription_for_user").querySelectorAll(".option-input").length === 0) {
				document.getElementById("another_text_sub").style.display = "none";
				document.getElementById("hr").style.display = "none";
			}

			if (document.getElementById("another_text_unsub").style.display === "none" && document.getElementById("unsubscription_for_user").querySelectorAll(".option-input").length > 0) {
				document.getElementById("another_text_unsub").style.display = "inline";
			}
			else if ((document.getElementById("another_text_unsub").style.display === "inline" || document.getElementById("another_text_unsub").style.display === "") && document.getElementById("unsubscription_for_user").querySelectorAll(".option-input").length === 0) {
				document.getElementById("another_text_unsub").style.display = "none";
				document.getElementById("hr").style.display = "none";
			}
			if (document.getElementById("hr").style.display === "none" && document.getElementById("subscription_for_user").querySelectorAll(".option-input").length > 0 && document.getElementById("unsubscription_for_user").querySelectorAll(".option-input").length > 0) {
				document.getElementById("hr").style.display = "block";
			}
		}

	};

	ourRequest.onerror = function () {
		console.log("Connection error");
	};
	ourRequest.setRequestHeader("Content-Type", "application/json; charset=UTF-8");
	ourRequest.send(JSON.stringify(send_data_actual));

}
var subs_submit = document.getElementById("subs_submit");

if (subs_submit) {
	subs_submit.addEventListener("click", subs_send_data);
}

window.onload = function () {
	if ((document.getElementById("another_text_sub").style.display === "inline" || document.getElementById("another_text_sub").style.display === "") && document.getElementById("subscription_for_user").querySelectorAll(".option-input").length === 0) {
		document.getElementById("another_text_sub").style.display = "none";
		document.getElementById("hr").style.display = "none";
	}


	else if ((document.getElementById("another_text_unsub").style.display === "inline" || document.getElementById("another_text_unsub").style.display === "") && document.getElementById("unsubscription_for_user").querySelectorAll(".option-input").length === 0) {
		document.getElementById("another_text_unsub").style.display = "none";
		document.getElementById("hr").style.display = "none";
	}
	if (document.getElementById("hr").style.display === "none" && document.getElementById("subscription_for_user").querySelectorAll(".option-input").length > 0 && document.getElementById("unsubscription_for_user").querySelectorAll(".option-input").length > 0) {
		document.getElementById("hr").style.display = "block";
	}
}


