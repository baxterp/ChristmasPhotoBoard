function ShowOverlay(message) {
  $("#snow").LoadingOverlay("show", {
    text: message,
    background: "rgba(39, 43, 48, 0.8)",
    textColor: "rgba(128, 128, 128, 1)",
  });
}

function HideOverlay() {
  $("#snow").LoadingOverlay("hide");
}

function ImageUpload() {
  if (!fileInput.files || fileInput.files.length === 0) {
    alert('Please select an image.');
    return;
  }

  const formData = new FormData();
  formData.append('file', fileInput.files[0]);

  ShowOverlay('Wait...');

  fetch('/Home/ImageUpload', {
    method: 'POST',
    body: formData
  })
    .then(response => {
      console.log(response);
      if (!response.ok) throw new Error('Network response was not ok');
      return response.text();
    })
    .then(summary => {
      fileInput.value = '';
      HideOverlay();
      window.location.href = '';
    })
    .catch(error => {
      console.log(error);
    });
}

$(function () {
  var lightboxImg = $("a.lightboxImg");
  lightboxImg.lightbox();

  $("#snow").fallingSnow();

  $('.lazy').Lazy();
});