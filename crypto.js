//криптография в js , генерация ключей. упаковка и распаковка ключа
async function importKey() {
    return window.crypto.subtle.importKey(
        "jwk", //может быть "jwk" or "raw"
        {   //пример jwk ключа, "raw" должен быть ArrayBuffer
            kty: "oct",
            k: "Y0zt37HgOx-BY7SQjYVmrqhPkO44Ii2Jcb9yydUDPfE",
            alg: "A256GCM",//AES-CTR
            ext: true,
        },
        {   //это варианты алгоритма, которые вы планируете использовать
            name: "AES-GCM",
        },
        false, //является ли ключ извлекаемым (т.е. может быть использован в exportKey)
        ["encrypt", "decrypt"] //может быть "encrypt", "decrypt", "wrapKey", or "unwrapKey"
    )
}
async function generateKey() {
    return window.crypto.subtle.generateKey(
        {
            name: "AES-GCM",
            length: 256, //может быть 128, 192, or 256
            hash: "SHA-512", //может быть "SHA-1", "SHA-256", "SHA-384", or "SHA-512", это входные данные
        },
        true, //является ли ключ извлекаемым (т.е. может быть использован в exportKey)
        ["encrypt", "decrypt"] //может быть "encrypt", "decrypt", "wrapKey", or "unwrapKey"
    )
}
async function encrypt(data, key, iv) {
    return window.crypto.subtle.encrypt(
        {
            name: "AES-GCM",
            iv: iv,
            tagLength: 128, //can be 32, 64, 96, 104, 112, 120 or 128 (default)
        },
        key, //из generateKey или importKey выше
        data //ArrayBuffer данных, которые вы хотите зашифровать
    )
}
async function decrypt(data, key, iv) {
    return window.crypto.subtle.decrypt(
        {
            name: "AES-GCM",
            iv: iv, //Вектор инициализации, который вы использовали для шифрования.
            tagLength: 128, //Длина метки, которую вы использовали для шифрования (если есть)
        },
        key, //из generateKey или importKey выше
        data //ArrayBuffer данных
    )
}
async function fun() {
    var keys = await importKey()
    var iv = new Uint8Array([188, 185, 57, 146, 246, 194, 114, 34, 12, 80, 198, 77])
    var enc = new TextEncoder();
    var data = enc.encode("Кальчевский")
    var encryptedData = await encrypt(data, keys, iv)
    var decryptedData = await decrypt(encryptedData, keys, iv)
    var enc = new TextDecoder("utf-8");
    document.write(enc.decode(encryptedData) + '<br>');
    document.write(enc.decode(decryptedData) + '<br>');
}
fun()

